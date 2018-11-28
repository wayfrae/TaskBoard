$(function () {
	$('[data-toggle="tooltip"]').tooltip()
})

$(document).ready(function () {
	$("span.fa-lock").hide();
});

$("span.fa-unlock").click(function () {
	$(this).hide();
	$(this).siblings("span.fa-lock").show();
});

$("span.fa-lock").click(function () {
	$(this).hide();
	$(this).siblings("span.fa-unlock").show();
});