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
            'command': '/mb',
            'text': command + ' ' + commandText,
            'response_url': 'https://hooks.slack.com/commands/1234/5678'

        }
    };
}

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

    describe("should fail if token is incorrect", function () {
        var call;
        before(function() {
            call = chakram.post(pathTo('/slashcommand'), null, getSlackSlashCommandData('activate', 'ABC123', false));
        });
        
        it("should 401", function() {
            return expect(call).to.have.status(401);
        });
    });

    describe("should fail if text is missing", function() {
        var call;
        before(function() {
            call = chakram.post(pathTo('/slashcommand'), null, getSlackSlashCommandData('activate'));
        });
        it("should have 200", function() {
            return expect(call).to.have.status(200);
        });
        it("should be ephemeral", function() {
            return expect(call).to.have.json('response_type', 'ephemeral');
        });
        it("should have failed", function() {
            return expect(call).to.have.json('status', 'failure');
        });
        it("from a validation error", function() {
            return expect(call).to.have.json('sub_status', 'validation');
        });
    });

    describe("should fail if code is incorrect length", function() {
        var call;
        before(function() {
            call = chakram.post(pathTo('/slashcommand'), null, getSlackSlashCommandData('activate', 'ABC12345598347FJ'));
        });
        it("should have 200", function() {
            return expect(call).to.have.status(200);
        });
        it("should be ephemeral", function() {
            return expect(call).to.have.json('response_type', 'ephemeral');
        });
        it("should have failed", function() {
            return expect(call).to.have.json('status', 'failure');
        });
        it("from a validation error", function() {
            return expect(call).to.have.json('sub_status', 'validation');
        });
    });

    describe("should fail if code is invalid", function() {
        var call;
        before(function() {
            call = chakram.post(pathTo('/slashcommand'), null, getSlackSlashCommandData('activate', 'ABC123'));
        });
        it("should have 200", function() {
            return expect(call).to.have.status(200);
        });
        it("should be ephemeral", function() {
            return expect(call).to.have.json('response_type', 'ephemeral');
        });
        it("should have failed", function() {
            return expect(call).to.have.json('status', 'failure');
        });
        it("from a not found error", function() {
            return expect(call).to.have.json('sub_status', 'not_found');
        });
    });

    describe("should activate valid code", function() {
        var call;
        before(function() {
            call = chakram.post(pathTo('/slashcommand'), null, getSlackSlashCommandData('activate', registrationCode));
        });
        it("should have 200", function() {
            return expect(call).to.have.status(200);
        });
        it("should be in channel", function() {
            return expect(call).to.have.json('response_type', 'in_channel');
        });
        it("should have succeeded", function() {
            return expect(call).to.have.json('status', 'success');
        });
    });
});

describe("should handle unknown commands", function() {
    var call;
    before(function() {
        call = chakram.post(pathTo('/slashcommand'), null, getSlackSlashCommandData('unknown-command'));
    });

    it("should have 200", function() {
        return expect(call).to.have.status(200);
    });

    it("should be ephemeral", function() {
        return expect(call).to.have.json('response_type', 'ephemeral');
    });

    it("should be error", function() {
        return expect(call).to.have.json('status', 'failure');
    });

    it("should be unknown command", function() {
        return expect(call).to.have.json('sub_status', 'unknown_command');
    });
});