const $ = require('shelljs')
const path = require('path')
require('../logging')

$.config.fatal = true
const root = path.join(__dirname, '..', '..')


module.exports.clear = function () {
    console.info('Clearing dist directory')

    $.rm('-rf', `${root}/dist`)
    $.mkdir('-p', `${root}/dist/app/`)
}

module.exports.build_sample_bot_app = function () {
    console.info('Building sample bot project')

    console.debug('Publishing web app project')
    $.exec(
        `docker run --rm ` +
        `--volume "${root}/src:/project/src" ` +
        `--volume "${root}/sample:/project/sample" ` +
        `--volume "${root}/dist/app:/app" ` +
        `--workdir /project/sample/Quickstart.AspNetCore/ ` +
        `microsoft/dotnet:2.1.402-sdk ` +
        `dotnet publish --configuration Release --output /app/`
    )
}
