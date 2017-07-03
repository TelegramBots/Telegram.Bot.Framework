var Script;
(function (Script) {
    var Stage = createjs.Stage;
    var Shape = createjs.Shape;
    var Ticker = createjs.Ticker;
    var Tween = createjs.Tween;
    var Ease = createjs.Ease;
    var Touch = createjs.Touch;
    var Text = createjs.Text;
    var isMobile = Touch.isSupported() === true;
    var canvas;
    var stage;
    var bg;
    var circle;
    var scoreText;
    var score = 0;
    var counter = 7;
    var counterInterval;
    var counterText;
    var hexValues = "0123456789ABCDEF";
    function generateColor() {
        var color = "#";
        for (var i = 0; i < 6; i++) {
            var i_1 = (Math.random() * 832740) % 16;
            color += hexValues.charAt(i_1);
        }
        return color;
    }
    function showScores() {
        if (!(this.readyState === 4 && this.status === 200)) {
            return;
        }
        var highScoreTexts = [];
        var highScores = JSON.parse(this.response);
        for (var i = 0; i < highScores.length; i++) {
            var score_1 = highScores[i];
            var text = new Text(score_1.score + "    " + score_1.user.first_name, "12pt sans", "yellow");
            text.x = (canvas.width / 2) - (text.getMeasuredWidth() / 2);
            text.y = 5 + (1.5 * text.getMeasuredLineHeight() * i);
            highScoreTexts[i] = text;
            stage.addChild(text);
        }
    }
    function sendScore() {
        var scoresUrl = getScoreUrl();
        var playerid = getPlayerId();
        var xhr = new XMLHttpRequest();
        var data = {
            playerId: playerid,
            score: score
        };
        xhr.open("POST", scoresUrl);
        xhr.send(JSON.stringify(data));
        xhr.addEventListener("readystatechange", function () {
            if (xhr.readyState === 4) {
                getScores();
            }
        });
    }
    function getScores() {
        var scoresUrl = getScoreUrl();
        var playerid = getPlayerId();
        var xhr = new XMLHttpRequest();
        xhr.addEventListener("load", showScores);
        xhr.open("GET", scoresUrl + ("?id=" + encodeURIComponent(playerid)));
        xhr.send();
    }
    function handleCounterInterval() {
        if (counter < 2) {
            clearInterval(counterInterval);
            stage.removeChild(circle, counterText);
            circle.removeEventListener("pressmove", beginMoveCircle);
            counter = null;
            counterInterval = null;
            if (botGameScoreUrlExists()) {
                sendScore();
            }
        }
        else {
            counter--;
            counterText.text = counter.toString();
        }
    }
    function beginMoveCircle(ev) {
        function setBgToDark() {
            bg.graphics
                .clear()
                .beginFill("#555")
                .drawRect(0, 0, canvas.width, canvas.height);
        }
        var distance = Math.sqrt(Math.pow(circle.x - ev.stageX, 2) +
            Math.pow(circle.y - ev.stageY, 2));
        distance = Math.floor(distance * Math.random() / 10);
        score += distance;
        scoreText.text = score.toString();
        var bgColor = "#458";
        var distanceThreshold = 12;
        if (isMobile) {
            distanceThreshold = 3;
        }
        if (distance > distanceThreshold) {
            bgColor = generateColor();
        }
        bg.graphics
            .clear()
            .beginFill(bgColor)
            .drawRect(0, 0, canvas.width, canvas.height);
        Tween.get(circle)
            .to({
            x: ev.stageX,
            y: ev.stageY
        }, undefined, Ease.backInOut)
            .call(setBgToDark);
    }
    function draw() {
        bg = new Shape();
        bg.graphics
            .beginFill('#555')
            .drawRect(0, 0, canvas.width, canvas.height);
        stage.addChild(bg);
        circle = new Shape();
        circle.graphics
            .beginFill("red")
            .beginStroke('yellow')
            .drawCircle(0, 0, 20);
        circle.x = canvas.width / 2;
        circle.y = canvas.height / 2;
        stage.addChild(circle);
        circle.addEventListener("pressmove", beginMoveCircle);
        scoreText = new Text(score.toString(), "14pt sans", "yellow");
        scoreText.x = 4;
        scoreText.y = 7;
        stage.addChild(scoreText);
        counterText = new Text(counter.toString(), "14pt sans", "#fff");
        counterText.x = canvas.width - counterText.getMeasuredWidth() - 10;
        counterText.y = 7;
        stage.addChild(counterText);
    }
    function resizeCanvas() {
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
    }
    function init() {
        canvas = document.getElementById("canvas");
        window.addEventListener("resize", resizeCanvas, false);
        resizeCanvas();
        stage = new Stage(canvas);
        Touch.enable(stage);
        Ticker.framerate = 60;
        Ticker.addEventListener("tick", function () { return stage.update(); });
        draw();
        counterInterval = setInterval(handleCounterInterval, 1 * 1000);
    }
    Script.init = init;
})(Script || (Script = {}));
window.addEventListener("load", Script.init);
//# sourceMappingURL=script.js.map