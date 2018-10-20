const $ = require('shelljs')
require('../logging')

$.config.fatal = true

const unit_test_script = require('./unit_tests')

try {
    unit_test_script.run_unit_tests('Release')
} catch (e) {
    console.error(e)
    process.exit(1)
}

console.info(`Tests succeeded.`)