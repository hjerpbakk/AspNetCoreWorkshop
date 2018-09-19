class HighScoreManager {
    constructor() {
        this.highScoreEndpoint = 'http://localhost:5000/api/highscore/';
    }

    setHighScore(score) {
        var request = new XMLHttpRequest();
        // Fire and forget web requests are real bad too. But this was a synchronous API,
        // so what can you do?
        request.open('POST', this.highScoreEndpoint, true);
        request.setRequestHeader("Content-type", "application/json");
        request.send(score);
    }

    getHighScore() {
        var request = new XMLHttpRequest();
        // Synchronous web requests are real bad. But this was a synchronous API,
        // so what can you do?
        request.open('GET', this.highScoreEndpoint, false);
        request.send(null);
        if (request.status === 200) {
            return parseInt(request.responseText, 10);
        }
        return 0;
    }
}
