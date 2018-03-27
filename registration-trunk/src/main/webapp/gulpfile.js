'use strict';

var gulp = require('gulp');
var plugins = require('gulp-load-plugins')();
var sass = require('gulp-sass');
var es = require('event-stream');
var usemin = require('gulp-usemin');
var uglify = require('gulp-uglify');
var minifyHtml = require('gulp-minify-html');
var minifyCss = require('gulp-clean-css');
var rev = require('gulp-rev');
var ngAnnotate = require('gulp-ng-annotate');
var print = require('gulp-print');
var del = require('del');

var paths = {
    appHtml: 'app/**/*.html',
    appJs: ['app/**/*.js'],
    appCss: ['assets/css'],
    appSass: ['assets/sass/*.scss'],
    index: './index.html',
    customIcons: 'assets/custom-icons/**/*',
    customIconsFonts: 'assets/custom-icons/fonts/*',
    langFiles: 'assets/locale/**/*',
    langFilesLoc: './assets/locale',
    langConfigFile: 'config/**.xlsx',
    distTemp: './dist',
    restructureFolders: ['*', '.*', '!dist', '!gulpfile.js', '!node_modules']
};

gulp.task('clean', function () {
    return del(paths.distTemp, { force: true });
});

gulp.task('copyFilesTemp', ['clean'], function () {
    return es.concat(
        gulp.src(paths.appHtml, { base: './' })
        .pipe(gulp.dest(paths.distTemp)),
        gulp.src(paths.langFiles)
        .pipe(gulp.dest(paths.distTemp + '/assets/locale/')),
        gulp.src(paths.index)
        .pipe(gulp.dest(paths.distTemp))
    );
});

gulp.task('sass', function () {
    gulp.src(paths.appSass)
      .pipe(sass({
        importer: require('node-sass').importer
      }))
      .pipe(gulp.dest(paths.appCss[0]));
});

gulp.task('usemin', ['copyFilesTemp'], function () {
    return gulp.src('./index.html')
        .pipe(usemin({
            css_app: [ minifyCss(), rev() ],
            css_vendor: [ minifyCss(), rev() ],
            html: [ minifyHtml({ empty: true }) ],
            js_app: [ngAnnotate(), uglify(),rev() ],
            js_vendor: [ uglify(), rev() ],
            inlinejs: [ uglify() ],
            inlinecss: [ minifyCss(), 'concat' ]
        }))
        .pipe(gulp.dest(paths.distTemp));
});

gulp.task('restructure.clean', ['createTemp'], function () {
  return del(paths.restructureFolders);
});

gulp.task('restructure.copy', ['createTemp', 'restructure.clean'],  function () {
  return gulp.src(paths.distTemp + '/**/*')
    .pipe(gulp.dest('./'));
});

gulp.task('restructure.finalize', ['restructure.copy'], function () {
  return del(paths.distTemp);
});

gulp.task('createTemp', ['sass', 'copyFilesTemp', 'usemin']);

gulp.task('build', ['createTemp', 'restructure.clean', 'restructure.copy', 'restructure.finalize']);

gulp.task('local', ['createTemp']);
