exports.get_deployment_settings = () => {
    const jsonValue = process.env['DEPLOY_SETTINGS_JSON']
    let settings;
    try {
        settings = JSON.parse(jsonValue)
    } catch (e) {
        throw `Value of "DEPLOY_SETTINGS_JSON" environment variable is not valid JSON.`
    }

    return settings
}
