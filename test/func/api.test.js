chakram = require('chakram');
expect = chakram.expect;

var baseUrl = process.env.BASE_URL == null ? "http://localhost:7071/api" : process.env.BASE_URL;

function pathTo(path) {
    return baseUrl + path
}

describe("Calling Play", function() {

    var playCall;

    before("Call Play", function() {
        playCall = chakram.post(pathTo('/play'));
    });

    it("Should Return Status Code 200", function () {
        return expect(playCall).to.have.status(200);
    });
});