mergeInto(LibraryManager.library, {

    TryLockOrientation: function (){
        var orientation = 'landscape';
        lockAllowed = window.screen.lockOrientation(orientation);
        if (!lockAllowed)
        {
            console.log('simple lock not allowed trying other lock function...');
            var lockFunction =  window.screen.orientation.lock;
            if (lockFunction.call(window.screen.orientation, orientation)) {
                console.log('Other Orientation locked');
            } else {
                console.error('There was a problem in locking the orientation');
            }
        }
        else{
            console.log('Simple Orientation locked');
        }
    }

    IsMobile: function(){
         return UnityLoader.SystemInfo.mobile;
    },

    GoFullscreen: function(){
        var viewFullScreen = document.getElementById('#canvas');

        var orientation = (screen.orientation || {}).type || screen.mozOrientation || screen.msOrientation;

        var ActivateFullscreen = function()
        {
            if(orientation == "landscape-primary"){
                if (viewFullScreen.requestFullscreen) /* API spec */
                {  
                    viewFullScreen.requestFullscreen();
                    screen.orientation.lock("landscape-primary");
                }
                else if (viewFullScreen.mozRequestFullScreen) /* Firefox */
                {
                    viewFullScreen.mozRequestFullScreen();
                    screen.mozLockOrientation.lock("landscape-primary");
                }
                else if (viewFullScreen.webkitRequestFullscreen) /* Chrome, Safari and Opera */
                {  
                    viewFullScreen.webkitRequestFullscreen();
                    screen.orientation.lock("landscape-primary");
                }
                else if (viewFullScreen.msRequestFullscreen) /* IE/Edge */
                {  
                    viewFullScreen.msRequestFullscreen();
                    screen.msLockOrientation.lock("landscape-primary");
                }
                viewFullScreen.removeEventListener('touchend', ActivateFullscreen);    
            }
        }
        viewFullScreen.addEventListener('touchend', ActivateFullscreen, false);
    },

    CheckOrientation: function(){
        var orientation = (screen.orientation || {}).type || screen.mozOrientation || screen.msOrientation;

        if(orientation == "landscape-primary")
        {
            return true;
        }
        else
        {
            return false;
        }
    },
});