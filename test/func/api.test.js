chakram = require('chakram');
uuid = require('uuid/v4');
expect = chakram.expect;

var baseUrl = process.env.BASE_URL == null ? "http://localhost:7071/api" : process.env.BASE_URL;
var testType = process.env.TEST_TYPE == null ? "local" : process.env.TEST_TYPE
var slackVerificationToken = process.env.SlackVerificationToken

function pathTo(path) {
    return baseUrl + path;
}

function getSlackSlashCommandData(command, commandText, valid) {
    return {
        form: {
            'token': (valid === false ? 'ABC123' : slackVerificationToken),
            'team_id': 'T0001',
            'team_domain': 'ostusa.com',
            'enterprise_id': 'E0001',
            'enterprise_name': 'Globular Construct Inc',
            'channel_id': 'C2147483705',
            'channel_name': 'test',
            'user_id': 'U2147483697',
            'user_name': 'Steve',
            'command': command,
            'text': commandText,
            'response_url': 'https://hooks.slack.com/commands/1234/5678'

        }
    };
}

describe(testType + " - Calling Play", function () {

    var playCall;

    before("Call Play", function () {
        playCall = chakram.post(pathTo('/play'));
    });

    it("should return status code 200", function () {
        return expect(playCall).to.have.status(200);
    });
});

describe(testType + " - Creating a Device", function () {
    var deviceCall;
    var id = uuid();
    var code = id.substring(30).toUpperCase();
    this.slow(500);

    var headers = {
        'Authorization': 'BetaKey ' + process.env.BetaKey
    }

    before("Create Device", function () {
        deviceCall = chakram.put(pathTo('/device'), {
            "DeviceId": id
        }, {
            "headers": headers
        });
    });

    it("should return status code 200", function () {
        return expect(deviceCall).to.have.status(200);
    });

    it("should have a registration code", function () {
        return expect(deviceCall).to.have.json('RegistrationCode', code)
    });
})

describe(testType + " - Creating a Device - Errors", function () {
    var deviceCAll;

    var goodHeaders = {
        'Authorization': 'BetaKey ' + process.env.BetaKey
    }

    var badHeaders = {
        'Authorization': 'BetaKey ABC123'
    }

    it("should 401 without BetaKey", function () {
        deviceCall = chakram.put(pathTo('/device'));
        return expect(deviceCall).to.have.status(401);
    });

    it("should 401 with Invalid BetaKey", function () {
        deviceCall = chakram.put(pathTo('/device'), {}, {
            "headers": badHeaders
        });
        return expect(deviceCall).to.have.status(401);
    });

    it("should 400 on missing body", function () {
        deviceCall = chakram.put(pathTo('/device'), null, {
            "headers": goodHeaders
        });
        return expect(deviceCall).to.have.status(400);
    });

    it("should 400 on missing DeviceId", function () {
        deviceCall = chakram.put(pathTo('/device'), {}, {
            "headers": goodHeaders
        });
        return expect(deviceCall).to.have.status(400);
    });

    it("should 400 on invalid Guid", function () {
        deviceCall = chakram.put(pathTo('/device'), {
            "DeviceId": "ABC123"
        }, {
            "headers": goodHeaders
        });
        return expect(deviceCall).to.have.status(400);
    });
});

describe(testType + " - Activate Device", function () {

    var deviceId = uuid();
    var registrationCode;
    before("Setup Registration Call", function() { 
        var registrationCall = chakram.put(pathTo('/device'), {
            "DeviceId": deviceId
        }, {
            "headers": {
                'Authorization': 'BetaKey ' + process.env.BetaKey
            }
        });
        return registrationCall.then(function(response) {
            registrationCode = response.body.RegistrationCode;
        });
    });

    it("should 401 if token is incorrect", function () {
        var call = chakram.post(pathTo('/device/activate'), null, getSlackSlashCommandData('activate', 'ABC123', false));
        return expect(call).to.have.status(401);
    });

    it("should 400 if text is missing", function() {
        var call = chakram.post(pathTo('/device/activate'), null, getSlackSlashCommandData('activate'));
        return expect(call).to.have.status(400);
    });

    it("should 400 if code is invalid", function () {
        var call = chakram.post(pathTo('/device/activate'), null, getSlackSlashCommandData('activate', 'ABC123'));
        return expect(call).to.have.status(400);
    });

    it("should successfully be able to be activated", function () {
        
        var call = chakram.post(pathTo('/device/activate'), null, getSlackSlashCommandData('activate', registrationCode));
        return expect(call).to.have.status(200);
    });
});