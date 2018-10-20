const chalk = require('chalk');
const $ = require('shelljs')

if (process.env['TRAVIS'] && process.env['CI']) {
    console.info = m => $.echo("\033[1;34m", m, "\033[0m")
    console.debug = m => $.echo("\033[0;32m", m, "\033[0m")
    console.warn = m => $.echo("\033[1;33m", m, "\033[0m")
    console.error = m => $.echo("\033[1;31m", m, "\033[0m")
} else {
    console.info = m => console.log(chalk.blue.bold(m))
    console.debug = m => console.log(chalk.green.bold(m))
    console.warn = m => console.log(chalk.yellow.bold(m))
    console.error = m => console.log(chalk.red.bold(m))
}