var popup1 = document.getElementById("popup1");
var open_popup1 = document.getElementById("dropdown-content").children[0];
var close_popup1 = popup1.children[0].children[0];

var popup2 = document.getElementById("popup2");
var open_popup2 = document.getElementById("dropdown-content").children[1];
var close_popup2 = popup2.children[0].children[0];

open_popup1.onclick = function () {
	popup1.style.display = "block";
}

close_popup1.onclick = function () {
	popup1.style.display = "none";
}

open_popup2.onclick = function () {
	popup2.style.display = "block";
}

close_popup2.onclick = function () {
	popup2.style.display = "none";
}

window.onclick = function (event) {
	if (event.target == popup1) {
		popup1.style.display = "none";
	} else if (event.target == popup2) {
		popup2.style.display = "none";
	}
}