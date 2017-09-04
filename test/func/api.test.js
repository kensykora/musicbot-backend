chakram = require('chakram');
expect = chakram.expect;

var baseUrl = process.env.BASE_URL == null ? "http://localhost:7071/api" : process.env.BASE_URL;
var testType = process.env.TEST_TYPE == null ? "local" : process.env.TEST_TYPE

function pathTo(path) {
    return baseUrl + path
}

describe(testType + " - Calling Play", function() {

    var playCall;

    before("Call Play", function() {
        playCall = chakram.post(pathTo('/play'));
    });

    it("should return status code 200", function () {
        return expect(playCall).to.have.status(200);
    });
});