chakram = require('chakram');
uuid = require('uuid/v4');
expect = chakram.expect;

var baseUrl = process.env.BASE_URL == null ? "http://localhost:7071/api" : process.env.BASE_URL;
var testType = process.env.TEST_TYPE == null ? "local" : process.env.TEST_TYPE
var slackVerificationToken = process.env.SlackVerificationToken

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
    var deviceCall;
    var id = uuid();
    var code = id.substring(30);
    this.slow(500);

    var headers = {
        'Authorization': 'BetaKey ' + process.env.BetaKey
    }

    before("Create Device", function() {
        deviceCall = chakram.put(pathTo('/device'), { "DeviceId": id }, { "headers": headers });
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

    var goodHeaders = {
        'Authorization': 'BetaKey ' + process.env.BetaKey
    }

    var badHeaders = {
        'Authorization': 'BetaKey ABC123'
    }

    it("should 401 without BetaKey", function() {
        deviceCall = chakram.put(pathTo('/device'));
        return expect(deviceCall).to.have.status(401);
    });

    it("should 401 with Invalid BetaKey", function() {
        deviceCall = chakram.put(pathTo('/device'), { }, { "headers": badHeaders });
        return expect(deviceCall).to.have.status(401);
    });

    it("should 400 on missing body", function() {
        deviceCall = chakram.put(pathTo('/device'), null, { "headers": goodHeaders });
        return expect(deviceCall).to.have.status(400);
    });

    it("should 400 on missing DeviceId", function() {
        deviceCall = chakram.put(pathTo('/device'), { }, { "headers": goodHeaders });
        return expect(deviceCall).to.have.status(400);
    });

    it("should 400 on invalid Guid", function() {
        deviceCall = chakram.put(pathTo('/device'), { "DeviceId": "ABC123" }, { "headers": goodHeaders });
        return expect(deviceCall).to.have.status(400);
    });
});

describe(testType + " - Activate Device", function() {
    it("should only be called from slack", function() {
        var call = chakram.post(pathTo('/device/activate'), null, { form : { 'token': slackVerificationToken }});
        return expect(call).to.have.status(200);
    });

    it("should 401 if token is incorrect", function() {
        var call = chakram.post(pathTo('/device/activate'), null, { form : { 'token': 'ABC123' }});
        return expect(call).to.have.status(401);
    });

    it("should 401 if token is missing", function() {
        var call = chakram.post(pathTo('/device/activate'), null, { form : {  }});
        return expect(call).to.have.status(401);
    });
});