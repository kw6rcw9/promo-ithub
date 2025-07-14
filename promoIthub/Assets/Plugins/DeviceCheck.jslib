mergeInto(LibraryManager.library, {
  IsMobileDevice: function () {
    if (typeof navigator !== 'undefined') {
      var ua = navigator.userAgent || navigator.vendor || window.opera;
      ua = ua.toLowerCase();
      return /android|iphone|ipad|ipod|mobile|touch/.test(ua) ? 1 : 0;
    }
    return 0;
  }
});
