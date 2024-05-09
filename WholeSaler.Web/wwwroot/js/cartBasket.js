 

function updateCartCount() {
    
var userId = @Html.Raw(Json.Serialize(userId));
var visitorId = @Html.Raw(Json.Serialize(visitorId));
        var url = '';
        if (userId !== '') {
            url = `https://localhost:7185/api/shoppingcart/getcart/${userId}`;
            console.log("if çalıştı");
        } else {
            url = `https://localhost:7185/api/shoppingcart/getcart/${visitorId}`;
            console.log("else çalıştı");
        }

        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                // Update cart count with the retrieved count
                $('#cart-count').text(data.products.count);
                console.log("cartbasket is running")
            },
            error: function (xhr, status, error) {
                console.error('Error fetching cart count:', error);
            }
        });
    }

    // Call the function to update cart count after page load
    $(document).ready(function () {
        updateCartCount();
    });