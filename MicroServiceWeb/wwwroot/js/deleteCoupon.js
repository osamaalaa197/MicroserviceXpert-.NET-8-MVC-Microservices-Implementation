
	// Function to handle deletion
	function deleteCoupon(couponId) {
		// Confirm the deletion if needed
		if (confirm("Are you sure you want to delete this coupon?")) {
		// AJAX request
		$.ajax({
			url: '/Coupon/DeleteCoupon',
			type: 'POST',
			data: { id: couponId },
			success: function (response) {
				// Check response and handle accordingly
				if (response.IsSuccess) {
					// Optionally show a success message
					toastr.success(response.Message);
					// Optionally reload or update the page
					window.location.reload();
				} else {
					// Optionally handle errors
					toastr.error(response.Message);
				}
			},
			error: function (xhr, status, error) {
				// Optionally handle errors
				console.error(xhr.responseText);
				alert('Error: ' + error);
			}
		});
		}
	}

	// Attach click event handler to the delete buttons
	$('.delete-coupon').click(function (e) {
		e.preventDefault(); // Prevent default link behavior
	var couponId = $(this).data('coupon-id'); // Get the coupon ID
	deleteCoupon(couponId); // Call deleteCoupon function
	});

