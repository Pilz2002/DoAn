$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var textQuantity = parseInt($('#quantity_value').text());

        if (textQuantity != null) {
            quantity = parseInt(textQuantity);
        }
        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (js) {
                if (js.success) {
                    $('#checkout_items').html(js.count);
                    alert(js.msg);
                }
            }
        });
    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa sản phẩm này khỏi giỏ hàng không?');
        if (conf == true) {
            $.ajax({
                url: '/shoppingcart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (js) {
                    if (js.success) {
                        $('#checkout_items').html(js.count);
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            });
        }
    });

    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có muốn xóa hết sản phẩm khỏi giỏ hàng không?');
        if (conf == true) {
            $.ajax({
                url: '/shoppingcart/DeleteAll',
                type: 'POST',
                success: function (js) {
                    if (js.success) {
                        LoadCart();
                    }
                }
            });
        }
    });

    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = $('#quantity_' + id).val();
        $.ajax({
            url: '/shoppingcart/update',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (js) {
                if (js.success) {
                    LoadCart();
                }
            }
        });
    });

});

function ShowCount() {
    $.ajax({
        url: '/shoppingcart/showcount',
        type: 'GET',
        success: function (js) {
            $('#checkout_items').html(js.count);
        }
    });
}

function LoadCart() {
    $.ajax({
        url: '/shoppingcart/partial_item_cart',
        type: 'GET',
        success: function (js) {
            $('#load_data').html(js);
            /*location.reload();*/
        }
    });
}
