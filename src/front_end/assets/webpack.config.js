"use strict";
var webpack = require('webpack');
var path = require('path');

var loaders = [];

// global css
loaders.push({
    test: /\.css$/,
    exclude: /[\/\\]src[\/\\]/,
    loaders: [
		'style?sourceMap',
		'css'
    ]
});

// local scss modules
loaders.push({
    test: /\.scss$/,
    exclude: /[\/\\](node_modules|bower_components|public\/)[\/\\]/,
    loaders: [
		'style?sourceMap',
		'css?modules&importLoaders=1&localIdentName=[path]___[name]__[local]___[hash:base64:5]',
		'sass'
    ]
});

// local css modules
loaders.push({
    test: /\.css$/,
    exclude: /[\/\\](node_modules|bower_components|public\/)[\/\\]/,
    loaders: [
		'style?sourceMap',
		'css?modules&importLoaders=1&localIdentName=[path]___[name]__[local]___[hash:base64:5]'
    ]
});

loaders.push({
    test: /\.html$/,
    loader: 'mustache'
    // loader: 'mustache?minify'
    // loader: 'mustache?{ minify: { removeComments: false } }'
    // loader: 'mustache?noShortcut'
});

module.exports = {
    entry: [
		'./src/app.js' // your app's entry point
    ],

    devtool: process.env.WEBPACK_DEVTOOL || 'eval-source-map',

    output: {
        publicPath: '../wwwroot/scripts/',
        path: '../wwwroot/scripts/',
        filename: 'bundle.js'
    },

    resolve: {
        extensions: ['', '.js']
    },

    module: {
        loaders
    }
};