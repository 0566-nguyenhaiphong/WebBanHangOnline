$(document).ready(function () {
    ShowCount()
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id')
        var quantity = 1
        var tQuantity = $('#quantity_value').text()
        if (tQuantity != "") {
            quantity = parseInt(tQuantity)
        }
      
        $.ajax({
            url: '/shoppingcart/AddToCart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);
                    showSuccessToast()
                }
            }
        });
    });
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id')
        var conf = confirm('Bạn có muốn xóa sản phẩm không ?')
        if (conf) {
            $.ajax({
                url: '/shoppingcart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove()
                        LoadCart()
                    }
                }
            })
        }
        
    })
    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        var id = $(this).data('id')
        var quantity = $('#Quantity_' + id).val()
        UpdateCart(id, quantity)

    });
    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var conf = confirm('Bạn có muốn xóa giỏ hàng không ?')
        if (conf) {         
            DeleteAll()
        }

    })
    //$('body').on('click', '.submitBtn', function (e) {
    //    $.ajax({
    //        url: '/ShoppingCart/CheckOut',
    //        type: 'POST',
    //        data: $('#myForm').serialize(),
    //        success: function (rs) {
    //            if (rs.success) {

    //                // Handle success (e.g., show a success message)
    //                showSuccessToast();
    //            }
    //        },
    //        error: function (rs) {
    //            if (!rs.success) {

    //                // Handle error (e.g., show an error message)
    //                showErrorToast();
    //            }

    //        }
    //    });
    //});

})
function ShowCount() {
    $.ajax({
        url: '/shoppingcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.Count);
        }
    });
}
function UpdateCart(id, quantity) {
    $.ajax({
        url: '/shoppingcart/UpdateCart',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                LoadCart()
            }
        }
    });
}

function DeleteAll() {
    $.ajax({
        url: '/shoppingcart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.Success) {
                $('#checkout_items').html(rs.Count);
                LoadCart()
            }
        }
    });
}
function LoadCart() {
    $.ajax({
        url: '/shoppingcart/Partial_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}
function showSuccessToast(rs) {
    toast({
        title: "Thành công!",
        message: "Mua hàng thành công",
        type: "success",
        duration: 3000
    });
}



function showErrorToast(rs) {
    toast({
        title: "Thất bại!",
        message: "Có lỗi xảy ra, vui lòng liên hệ quản trị viên.",
        type: "error",
        duration: 3000
    });

}
// Toast function
function toast({ title = "", message = "", type = "info", duration = 3000 }) {
    const main = document.getElementById("toast");
    if (main) {
        const toast = document.createElement("div");

        // Auto remove toast
        const autoRemoveId = setTimeout(function () {
            main.removeChild(toast);
        }, duration + 1000);

        // Remove toast when clicked
        toast.onclick = function (e) {
            if (e.target.closest(".toast__close")) {
                main.removeChild(toast);
                clearTimeout(autoRemoveId);
            }
        };

        const icons = {
            success: "fa fa-check",
            info: "fas fa-info-circle",
            warning: "fas fa-exclamation-circle",
            error: "fas fa-exclamation-circle"
        };
        const icon = icons[type];
        const delay = (duration / 1000).toFixed(2);

        toast.classList.add("toast", `toast--${type}`);
        toast.style.animation = `slideInLeft ease .3s, fadeOut linear 1s ${delay}s forwards`;

        toast.innerHTML = `
                      <div class="toast__icon">
                          <i class="${icon}"></i>
                      </div>
                      <div class="toast__body">
                          <h3 class="toast__title">${title}</h3>
                          <p class="toast__msg">${message}</p>
                      </div>
                      <div class="toast__close">
                          <i class="fa fa-times"></i>
                      </div>
                  `;
        main.appendChild(toast);
    }
}

