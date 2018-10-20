const $ = require('shelljs')
require('../logging')

$.config.fatal = true


exports.deploy = function (source, target, user, pass) {
    console.info(`pushing docker local image ${source} to ${target}`)

    $.exec(`docker tag ${source} ${target}`)
    $.exec(`docker login --username ${user} --password ${pass}`)
    $.exec(`docker push ${target}`)
    $.exec('docker logout')
}
