const $ = require('shelljs')
const path = require('path')
require('../logging')

$.config.fatal = true
const root = path.join(__dirname, '..', '..')


module.exports.run_unit_tests = function (configuration) {
    console.info(`Running unit tests with configuartion ${configuration}`)

    $.exec(
        `docker run --rm ` +
        `--volume "${root}:/project" ` +
        `--workdir /project/test/UnitTests.NetCore/ ` +
        `microsoft/dotnet:2.1.402-sdk ` +
        `dotnet test --configuration ${configuration} --framework netcoreapp2.1`
    )
}
