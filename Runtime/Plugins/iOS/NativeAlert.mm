#import <UIKit/UIKit.h>

extern "C" {
    void _ShowAlert(const char* title, const char* message) {
        NSString *titleString = [NSString stringWithUTF8String:title];
        NSString *messageString = [NSString stringWithUTF8String:message];

        UIViewController *rootViewController = [UIApplication sharedApplication].keyWindow.rootViewController;

        UIAlertController *alert = [UIAlertController alertControllerWithTitle:titleString
                                                                       message:messageString
                                                                preferredStyle:UIAlertControllerStyleAlert];

        UIAlertAction *okAction = [UIAlertAction actionWithTitle:@"OK"
                                                           style:UIAlertActionStyleDefault
                                                         handler:nil];

        [alert addAction:okAction];

        dispatch_async(dispatch_get_main_queue(), ^{
            [rootViewController presentViewController:alert animated:YES completion:nil];
        });
    }
}
