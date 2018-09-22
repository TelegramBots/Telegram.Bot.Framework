const $ = require('shelljs')
require('../logging')

$.config.fatal = true
const root = `${__dirname}/../..`


module.exports.build_image = function () {
    const image_name = 'quickstart:latest'
    console.info(`building Docker Image "${image_name}"`)

    console.debug('copying Dockerfile')
    $.cp(`${root}/scripts/build/Heroku.Dockerfile`, `${root}/dist/Dockerfile`)

    console.debug('building docker image')
    $.exec(`docker build -t ${image_name} ${root}/dist/`)
}
