const $ = require('shelljs')
require('../logging')

$.config.fatal = true


function validate_args(...args) {
    if (!args.every(x => x && x.length)) {
        throw `All the required parameters should have value.`
    }
}

function push_image_to_heroku(app, source, dyno, user, token) {
    console.info('pushing docker image to heroku')

    console.debug('connecting to heroku docker registry')
    $.exec(`docker login --username ${user} --password ${token} registry.heroku.com`)

    console.debug('tagging the image')
    $.exec(`docker tag ${source} registry.heroku.com/${app}/${dyno}`)

    console.debug('pushing the image')
    $.exec(`docker push registry.heroku.com/${app}/${dyno}`)
}

function release_heroku_app(app, source, dyno, token) {
    console.info('deploying the image to heroku dyno')

    console.debug(`getting docker image ID`)
    const image_id = $.exec(`docker inspect ${source} --format={{.Id}}`).stdout.trim()

    console.debug(`upgrading to new release`)
    const post_data = JSON.stringify({
        updates: [{
            type: dyno,
            docker_image: image_id
        }]
    })

    $.exec(
        `curl -X PATCH https://api.heroku.com/apps/${app}/formation ` +
        `-H 'Authorization: Bearer ${token}' ` +
        `-H "Content-Type: application/json" ` +
        `-H "Accept: application/vnd.heroku+json; version=3.docker-releases" ` +
        `-d ${JSON.stringify(post_data)}`
    )
}


exports.deploy = function (app, source, dyno, user, token) {
    validate_args(app, source, dyno, user, token)
    push_image_to_heroku(app, source, dyno, user, token)
    release_heroku_app(app, source, dyno, token)
}