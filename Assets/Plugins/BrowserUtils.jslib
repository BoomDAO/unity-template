var WebGLFunctions = {
    
    ToggleLoginIframe: function (show) {
        var iframe = document.getElementById("loginIframe");
        iframe.style.display = show === 1 ? "block" : "none";
    },
    
    IsMobileBrowser: function () {
        return (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent));
    }
};

mergeInto(LibraryManager.library, WebGLFunctions);