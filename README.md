Unlockable.unitypackage
=======================

An Unlockable Unity Package to integrate the Unlockable monetization API into your Unity apps!

For more information about Unlockable and its monetization tool for your mobile games, please visit http://unlockable.com

Implementation
=======================
There are two implementations in the package. The `WWW` implementation is preferred as it supports SSL. We understand that Unity’s `WWW` implementation may not be as development friendly, but we've provided a regular `HTTPWebRequest` class for developers to test against. It is preferred that any project released for production implements the `WWW` solution.

1) To navigate to the correct ad url, make sure that you call `Unlockable.RequestInventory( )` with all the right credentials.

2) Make sure you send the appropriate Advertiser Identifier (`IDFA`/`ADID`) for your iOS/Android platform. Please refer to the respective Android and iOS documentation how to retrieve these.

3) Select the correct `UserAgent`, this is important for Unlockable to determine what ads are served to which devices. Failing to supply the right `UserAgent` may cause unexpected behaviour.

3) After making a successful call you will have a `game_url` with which you may now open into a WebView-type object.

4) The Unlockable advertising experience will now be displayed. After completion, the Webview may now be closed. Any handling of success or failure will be done Api-to-Api.

NOTE: Unlockable does currently not provide a built-in WebView component. 3rd Party developers must supply their own solution. However, Unlockable has builtin support for [UniWebView](http://uniwebview.onevcat.com/).
