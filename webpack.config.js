const path = require('path');
const nodeExternals = require('webpack-node-externals');

module.exports = {
    entry: {
        index: './wwwroot/src/ts/index.ts',
        home: './wwwroot/src/ts/site.ts'
        },
    devtool: 'inline-source-map',
    target: 'node',
    externals: [nodeExternals()],
    externalsPresets: {
        node: true
    },
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js']
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'CryptonOnboarding', 'wwwroot', 'js')
    }
}