const gulp = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const clean = require('gulp-clean');
const uglify = require('gulp-uglify');
const rename = require('gulp-rename');
const webpack = require('webpack-stream');
const htmlmin = require('gulp-htmlmin');

// compile scss to css
gulp.task('sass', function () {
    return gulp.src('./wwwroot/src/sass/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/dist/css'));
});

gulp.task('email', function () {
    return gulp.src('./wwwroot/src/email/**/*.html')
        .pipe(htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest('./wwwroot/dist/email/'));
});

gulp.task('ts', function () {
   return gulp.src('./wwwroot/ts/**/*.ts')
       .pipe(webpack(require('./webpack.config.js')))
       .pipe(gulp.dest('./wwwroot/dist/js/'));
});

gulp.task('js', function(){
    return  gulp.src('./wwwroot/src/js/**/*.js')
        .pipe(uglify())
        .pipe(rename({suffix: '.min'}))
        .pipe(gulp.dest('./wwwroot/dist/js/'));
});

// watch changes in scss files and run sass task
gulp.task('sass:watch', function () { gulp.watch('./wwwroot/src/sass/**/*.scss', gulp.series('sass')); });

gulp.task('ts:watch', function () { gulp.watch('./wwwroot/src/ts/**/*.ts', gulp.series('ts')); });

gulp.task('ts:clean', function () {
    return gulp.src('./wwwroot/dist/js/**/*.js', {read:false})
        .pipe(clean());
 });

 gulp.task('sass:clean', function () {
    return gulp.src('./wwwroot/dist/css/**/*.css', {read:false})
        .pipe(clean());
 });

 gulp.task('clean', gulp.parallel('ts:clean', 'sass:clean'));

gulp.task('watch', gulp.parallel('sass:watch', 'ts:watch'));

// default task
gulp.task('default', gulp.series('clean', 'sass', 'ts', 'js'));