require('../logging')


function get_environment_name() {
    console.info('verifying environment name')

    const environment_name = process.argv[process.argv.length - 1]
    if (environment_name && environment_name.length) {
        console.debug(`environment is ${environment_name}.`)
        return environment_name
    } else {
        throw `No environment name is passed.\n` +
            `\tExample: node ci/deploy Staging`
    }
}

function get_deployments_for_env(environment_name) {
    console.info(`finding deployments for environment ${environment_name}.`)

    const jsonValue = process.env['DEPLOY_SETTINGS_JSON']
    let deployment_map;
    try {
        deployment_map = JSON.parse(jsonValue)
    } catch (e) {
        throw `Value of "DEPLOY_SETTINGS_JSON" environment variable is not valid JSON.`
    }

    const env_deployments = deployment_map[environment_name];
    if (!env_deployments) {
        throw `There are no field for environment ${environment_name} in "DEPLOY_SETTINGS_JSON" value.`
    }
    if (!(Array.isArray(env_deployments) && env_deployments.length)) {
        console.warn(`There are deployments specified for environment ${environment_name}.`)
    }

    console.debug(`${env_deployments.length || 0} deployments found.`)

    return env_deployments
}

function deploy(environment_name, deployment) {
    console.info(`deploying to ${deployment.type} for environment ${environment_name}.`)
    const docker = require('./deploy_docker_registry')
    const heorku = require('./deploy_heroku')

    if (deployment.type === 'docker') {
        docker.deploy(
            deployment.options.source,
            deployment.options.target,
            deployment.options.user,
            deployment.options.pass
        )
    } else if (deployment.type === 'heroku') {
        heorku.deploy(
            deployment.options.app,
            deployment.options.source,
            deployment.options.dyno,
            deployment.options.user,
            deployment.options.token
        )
    } else {
        throw `Invalid deployment type ${deployment.type}.`
    }
}


try {
    const environment_name = get_environment_name()
    get_deployments_for_env(environment_name)
        .forEach(d => deploy(environment_name, d))
} catch (e) {
    console.error(e)
    process.exit(1)
}
