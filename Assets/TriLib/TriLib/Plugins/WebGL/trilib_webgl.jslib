mergeInto(LibraryManager.library, {
	WebGL_SetupUploadEvent: function(gameObjectName, formName, inputName) {		
		var formNameString = Pointer_stringify(formName);
		var inputNameString = Pointer_stringify(inputName);
		var gameObjectNameString = Pointer_stringify(gameObjectName);
		document.forms[formNameString][inputNameString].addEventListener("change", function (e) {
			var files = e.target.files;
			if (files.length != 0) {
				var file = files[0];
				var fileReader = new FileReader();
				fileReader.onload = (function() {
					var pointer = _malloc(fileReader.result.byteLength);
					var dataHeap = new Uint8Array(HEAPU8.buffer, pointer, fileReader.result.byteLength);
					dataHeap.set(new Uint8Array(fileReader.result));
					var json = JSON.stringify({Name: file.name, Size: fileReader.result.byteLength, Pointer: pointer});
					SendMessage(gameObjectNameString, "UploadFieldUpdated", json);
					_free(pointer);
				});
				fileReader.readAsArrayBuffer(file);
			}
		}, false);
	},
	WebGL_SetupPasteEvent: function(gameObjectName) {
		var gameObjectNameString = Pointer_stringify(gameObjectName);
		document.addEventListener("paste", function (event) {
			SendMessage(gameObjectNameString, "TextPasted", event.clipboardData.getData("Text"));
		});
	}
});