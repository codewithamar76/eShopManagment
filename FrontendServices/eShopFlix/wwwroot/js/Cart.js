function AddToCart(productID, unitPrice, Quantity) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: `/Cart/AddToCart/${productID}/${unitPrice}/${Quantity}`,
        success: function (data) {
            if (data != undefined && data.status === 'success') {
                $("#cartCount").text(data.cartCount);
                getCartCount();
                //$("#cartTotal").text(data.cartTotal.toFixed(2));
            }
        },
        error: function (xhr, status, error) {
            console.error("Error adding to cart:", error);
            alert("An error occurred while adding the item to the cart. Please try again.");
        }
    })
}

function deleteItem(id, cartId) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: `/Cart/DeleteItem/${id}/${cartId}`,
        success: function (data) {
            if (data > 0) {
                location.reload();
            }
        },
        error: function (xhr, status, error) {
            console.error("Error deleting item from cart:", error);
            alert(`An error occurred while deleting the item from the cart. Please try again. ${error}`);
        }
    });
}
function updateQuantity(id, cartId, quantity, currentQuantity) {
    if ((currentQuantity >= 1 && quantity == 1) || (currentQuantity >= 1 && quantity == -1)) {
        
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: `/Cart/UpdateQuantity/${id}/${cartId}/${quantity}`,
            success: function (data) {
                if (data > 0) {
                    location.reload();
                }
            },
            error: function (xhr, status, error) {
                console.error("Error updating cart item:", error);
                alert("An error occurred while updating the cart item. Please try again.");
            }
        });
    }
}

function getCartCount() {
    $.ajax({
        type: "GET",
        url: "/Cart/GetCartCount",
        success: function (data) {
            console.log(data)
            $("#cartCount").text(data);
            //$("#cartTotal").text(data.cartTotal.toFixed(2));
        },
        error: function (xhr, status, error) {
            console.error("Error fetching cart count:", error);
        }
    });
}

$(document).ready(function () {
    // Update the cart count on page load
    getCartCount();
});