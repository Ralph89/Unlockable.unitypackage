Unlockable.unitypackage
=======================

An Unlockable Unity Package to integrate the Unlockable monetization API into your Unity apps!

For more information about Unlockable and its monetization tool for your mobile games, please visit http://unlockable.com

Implementation
=======================
There are two implementations in the package. The WWW implementation is preferred as it supports SSL. We understand that Unity’s WWW implementation is less then ideal but have provided a regular HTTPWebRequest class for developers to test against. 

Please make sure that any project released for production implemented the WWW solution.

1) To navigate to the correct ad url, make sure that you call .RequestInventory() with all the right credentials.

2) Make sure you send the appropriate IDFA key. Please review the respective Android and iOS documentation how to retrieve these.

3) Select the correct UserAgent, this is important for Unlockable to determine what ads we server to which devices. Failing to supply the right agent may cause incoherent behaviour.

3) After making a successful call you will have a URL with which you may now open a “WebView”.

4) The ad will now be displayed, After completion of the ad the Webview may now be closed. Any handling of success or failure will be done Api-to-Api.

NOTE: Unlockable does currently not provide a built-in WebView component. 3rd Party developers must supply their own solution. However, Unlockable has builtin support for UniWebView.