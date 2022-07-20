// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');
tag.id = 'iframe-demo';
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
// 3. This function creates an <iframe> (and YouTube player)
//    after the API code downloads.
var player;
function onYouTubeIframeAPIReady() {
}
// 4. The API will call this function when the video player is ready.
function onPlayerReady(event) {
    event.target.playVideo();
}
// 5. The API calls this function when the player's state changes.
//    The function indicates that when playing a video (state=1),
//    the player should play for six seconds and then stop.
var done = false;
function onPlayerStateChange(event) {
    if (event.data == YT.PlayerState.PLAYING && !done) {
        setTimeout(stopVideo, 6000);
        done = true;
    }
}
function stopVideo() {
    player.stopVideo();
}

var unityWebView =
{
    loaded: [],
    playYoutubeID: function (id){
        window.alert ("play youtube.");
        player = new YT.Player('webview_WebViewObject', {
            events: {
                'onReady': onPlayerReady,
                'onStateChange': onPlayerStateChange
            }
        });
    },
    init: function (name) {
        $containers = $('.webviewContainer');
        if ($containers.length === 0) {
            $('<div style="position: absolute; left: 0px; width: 100%; height: 100%; top: 0px; pointer-events: none;"><div class="webviewContainer" style="overflow: hidden; position: relative; width: 100%; height: 100%; z-index: 1;"></div></div>')
                .appendTo($('#unity-container'));
        }
        var $last = $('.webviewContainer:last');
        var clonedTop = parseInt($last.css('top')) - 100;
        var $clone = $last.clone().insertAfter($last).css('top', clonedTop + '%');
        var $iframe =
            $('<iframe style="position:relative; width:100%; height100%; border-style:none; display:none; pointer-events:auto;" frameborder="0" allowFullScreen="true" webkitallowfullscreen="true" mozallowfullscreen="true"></iframe>')
                //$('<iframe style="position:relative; width:100%; height100%; border-style:none; src="https://www.youtube.com/embed/dQw4w9WgXcQ"; pointer-events:auto;"></iframe>')

                .attr('id', 'webview_' + name)
                .appendTo($last)
                .on('load', function () {
                    $(this).attr('loaded', 'true');
                    var contents = $(this).contents();
                    var w = $(this)[0].contentWindow;
                    contents.find('a').click(function (e) {
                        var href = $.trim($(this).attr('href'));
                        if (href.substr(0, 6) === 'unity:') {
                            unityInstance.SendMessage(name, "CallFromJS", href.substring(6, href.length));
                            e.preventDefault();
                        } else {
                            w.location.replace(href);
                        }
                    });

                    contents.find('form').submit(function () {
                        $this = $(this);
                        var action = $.trim($this.attr('action'));
                        if (action.substr(0, 6) === 'unity:') {
                            var message = action.substring(6, action.length);
                            if ($this.attr('method').toLowerCase() == 'get') {
                                message += '?' + $this.serialize();
                            }
                            unityInstance.SendMessage(name, "CallFromJS", message);
                            return false;
                        }
                        return true;
                    });

                    unityInstance.SendMessage(name, "CallOnLoaded", location.href);
                });
    },

    sendMessage: function (name, message) {
        unityInstance.SendMessage(name, "CallFromJS", message);
    },

    setMargins: function (name, left, top, right, bottom) {
        var container = $('#unity-container');
        var width = container.width();
        var height = container.height();

        var lp = left / width * 100;
        var tp = top / height * 100;
        var wp = (width - left - right) / width * 100;
        var hp = (height - top - bottom) / height * 100;

        this.iframe(name)
            .css('left', lp + '%')
            .css('top', tp + '%')
            .css('width', wp + '%')
            .css('height', hp + '%');
    },

    setVisibility: function (name, visible) {
        if (visible)
            this.iframe(name).show();
        else
            this.iframe(name).hide();
    },

    loadURL: function (name, url) {
        this.iframe(name).attr('allowFullScreen', 'true');
        this.iframe(name).attr('webkitallowfullscreen', 'true');
        this.iframe(name).attr('mozallowfullscreen', 'true')
        this.iframe(name).parent().attr('allowFullScreen', 'true');
        this.iframe(name).parent().parent().attr('allowFullScreen', 'true');
        this.iframe(name).attr('loaded', 'false')[0].contentWindow.location.replace(url);
    },

    evaluateJS: function (name, js) {
        $iframe = this.iframe(name);
        if ($iframe.attr('loaded') === 'true') {
            $iframe[0].contentWindow.eval(js);
        } else {
            $iframe.on('load', function () {
                $(this)[0].contentWindow.eval(js);
            });
        }
    },

    destroy: function (name) {
        this.iframe(name).parent().parent().remove();
    },

    iframe: function (name) {
        return $('#webview_' + name);
    },
};
