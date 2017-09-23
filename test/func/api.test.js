chakram = require('chakram');
uuid = require('uuid/v4');
expect = chakram.expect;

var baseUrl = process.env.BASE_URL == null ? "http://localhost:7071/api" : process.env.BASE_URL;
var testType = process.env.TEST_TYPE == null ? "local" : process.env.TEST_TYPE

function pathTo(path) {
    return baseUrl + path;
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

describe(testType + " - Creating a Device", function() {
    var deviceCAll;
    var id = uuid();
    var code = id.substring(30);
    this.slow(500);

    before("Create Device", function() {
        deviceCall = chakram.put(pathTo('/device'), { "DeviceId": id });
    });

    it("should return status code 200", function() {
        return expect(deviceCall).to.have.status(200);
    });

    it("should have a registration code", function() {
        return expect(deviceCall).to.have.json('RegistrationCode', code)
    });
})

describe(testType + " - Creating a Device - Errors", function() {
    var deviceCAll;

    it("should 400 on missing body", function() {
        deviceCall = chakram.put(pathTo('/device'));
        return expect(deviceCall).to.have.status(400);
    });

    it("should 400 on missing DeviceId", function() {
        deviceCall = chakram.put(pathTo('/device', { }));
        return expect(deviceCall).to.have.status(400);
    });

    it("should 400 on invalid Guid", function() {
        deviceCall = chakram.put(pathTo('/device', { "DeviceId": "ABC123" }));
        return expect(deviceCall).to.have.status(400);
    });
})