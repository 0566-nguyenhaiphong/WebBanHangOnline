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
                    alert(rs.msg);
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
                       
                    }
                }
            });
        }
        
    });
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