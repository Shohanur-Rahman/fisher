var _ath = addToHomescreen;

// This is the special custom message for stock android, it has to be customized to your needs
var athMessages = {
	samsungAndroid: 'Custom message for Samsung Android. Click the menu hardware button and blah blah blah. This is an icon if needed: %icon',
	stockAndroid: 'Custom message for stock Android. Click the menu hardware button and blah blah blah. This is an icon if needed: %icon'
};

// Add stock browser compatibility
var _ua = window.navigator.userAgent;

_ath.isAndroidBrowser = _ua.indexOf('Android') > -1 && !(/Chrome\/[.0-9]*/).test(_ua);
_ath.isCompatible = _ath.isCompatible || _ath.isAndroidBrowser;
if ( _ath.OS == 'unsupported' && _ath.isAndroidBrowser ) {
	// additionally we check for some Samsung devices (not strictly needed)
	_ath.OS = (/ (GT-I9|GT-P7|SM-T2|GT-P5|GT-P3|SCH-I8)/).test(_ua) ? 'samsungAndroid' : 'stockAndroid';
}

_ath({
	message: _ath.OS in athMessages ? athMessages[_ath.OS] : '',

	// the followings are just for debug, customize the options to your needs
	skipFirstVisit: false,
	displayPace: false
});