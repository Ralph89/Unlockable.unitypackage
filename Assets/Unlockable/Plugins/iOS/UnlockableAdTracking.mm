//
//  UnlockableAdSupport.m
//  UnlockableAdSupport
//
//  Created by Kevin Toet on 24/11/14.
//
//

#import <AdSupport/ASIdentifierManager.h>

extern "C"
{
    bool adTrackingEnabled()
    {
        ASIdentifierManager *adIdentManager = [ASIdentifierManager sharedManager];

        return adIdentManager.advertisingTrackingEnabled;
    }
}
