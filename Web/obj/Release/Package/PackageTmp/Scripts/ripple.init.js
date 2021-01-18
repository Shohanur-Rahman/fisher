$(document).ready(function () {
	try {
		$('#world').ripples({
			resolution: 800,
			dropRadius: 20,
			perturbance: 0.04,
		});

	}
	catch (e) {
		$('.error').show().text(e);
	}

	$('.js-ripples-disable').on('click', function () {
		$('#world').ripples('destroy');
		$(this).hide();
	});

	$('.js-ripples-play').on('click', function () {
		$('#world').ripples('play');
	});

	$('.js-ripples-pause').on('click', function () {
		$('#world').ripples('pause');
	});

	// Automatic drops
	setInterval(function () {
		var $el = $('#world');
		var x = Math.random() * $el.outerWidth();
		var y = Math.random() * $el.outerHeight();
		var dropRadius = 40;
		var strength = 1 + Math.random() * 2;

		$el.ripples('drop', x, y, dropRadius, strength);
	}, 300);
})


