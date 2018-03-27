// Karma configuration
// Generated on Mon Mar 21 2016 11:01:55 GMT+0000 (GMT)

module.exports = function(config) {
    config.set({
  
      // base path that will be used to resolve all patterns (eg. files, exclude)
      basePath: '',
  
  
      // frameworks to use
      // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
      frameworks: ['jasmine'],
  
  
      // list of files / patterns to load in the browser
      files: [
        'node_modules/jquery/dist/jquery.js',
        'node_modules/angular/angular.js',
        'node_modules/angular-route/angular-route.js',
        'node_modules/angular-mocks/angular-mocks.js',
        'node_modules/angular-translate/dist/angular-translate.js',
        'node_modules/angular-translate/dist/angular-translate-loader-static-files/angular-translate-loader-static-files.js',
        'node_modules/angular-translate/dist/angular-translate-storage-cookie/angular-translate-storage-cookie.js',
        'node_modules/angular-translate/dist/angular-translate-storage-local/angular-translate-storage-local.js',
        'node_modules/angular-translate/dist/angular-translate-handler-log/angular-translate-handler-log.js',
        'node_modules/angular-cookies/angular-cookies.js',
        'node_modules/ui-select/dist/select.js',
        'node_modules/angular-ui-bootstrap/dist/ui-bootstrap-tpls.js',
        'node_modules/bootstrap/dist/js/bootstrap.min.js',
        'node_modules/moment/min/moment-with-locales.min.js',
        'node_modules/angular-dynamic-locale/src/tmhDynamicLocale.js',
        'app/*.js',
        'app/**/*.js',
        'app/**/**/*.js',
        'app/**/**/**/**/*.js',
        'tests/**/*.js',
        'tests/**/**/*.js',
        'app/**/**/*.html',
        'app/**/**/**/**/*.html'
      ],
  
  
      // list of files to exclude
      exclude: [
        
      ],
  
  
      // preprocess matching files before serving them to the browser
      // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
      preprocessors: {
        'app/**/**/*.html': 'ng-html2js',
        'app/**/**/*.js': 'coverage'
      },
  
      ngHtml2JsPreprocessor: {
      // setting this option will create only a single module that contains templates
      // from all the files, so you can load them all with module('templates')
        moduleName: 'templates'
      },
  
  
      // test results reporter to use
      // possible values: 'dots', 'progress'
      // available reporters: https://npmjs.org/browse/keyword/karma-reporter
      reporters: [
        'progress',
        'coverage'
        ],
  
  
      // web server port
      port: 9876,
  
  
      // enable / disable colors in the output (reporters and logs)
      colors: true,
  
  
      // level of logging
      // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
      logLevel: config.LOG_INFO,
  
  
      // enable / disable watching file and executing tests whenever any file changes
      autoWatch: true,
  
  
      // start these browsers
      // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
      browsers: [
          'PhantomJS'
          // 'Chrome',
          // 'Opera',
          // 'Firefox'
          ],
     coverageReporter: {
      dir: 'coverage',
      reporters: [
         {type: 'html', subdir: 'report-html'},
        //  {type: 'text-summary', subdir: 'report-text', file: 'coverage.txt'},
         {type: 'cobertura', subdir: 'report-xml'}
       ]
     },
  
      plugins: [
          'karma-phantomjs-launcher',
          'karma-jasmine',
          'karma-coverage',
          'karma-chrome-launcher',
          'karma-ng-html2js-preprocessor'
          // 'karma-opera-launcher',
          // 'karma-firefox-launcher'
      ],
  
      // Continuous Integration mode
      // if true, Karma captures browsers, runs the tests and exits
      singleRun: true,
  
  
      // Concurrency level
      // how many browser should be started simultaneous
      concurrency: Infinity
    })
}
  