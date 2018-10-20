const $ = require('shelljs')
const path = require('path')
require('../logging')

$.config.fatal = true
const root = path.resolve(`${__dirname}/../..`)


try {
    console.info(`building Docker image`)

    console.debug('building the solution with "Release" configuration and "quickstart:latest" tag')
    $.cd(root)
    $.exec(
        `docker build --tag "quickstart:latest" --no-cache .`
    )
} catch (e) {
    console.error(e)
    process.exit(1)
}

console.info(`✅ Build succeeded`)
