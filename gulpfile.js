// import gulp from 'gulp';
const gulp = require('gulp');
const sass = require('gulp-sass')(require('sass'));
// import ts from 'gulp-typescript';
// import sourcemaps from 'gulp-sourcemaps';
// import uglify from 'gulp-uglify';
// import { deleteAsync } from 'del';
// import webpack from 'webpack-stream'
const webpack = require('webpack-stream');

// compile scss to css
gulp.task('sass', function () {
    return gulp.src('./wwwroot/src/sass/site.scss')
        .pipe(sass({ outputStyle: 'compressed' }).on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot/dist/css'));
});


// const tsProject = ts.createProject('tsconfig.json');

//compile ts to js
// gulp.task('ts', function () {
//     return tsProject.src()
//         .pipe(sourcemaps.init())
//         .pipe(tsProject())
//         //.pipe(uglify())
//         .pipe(sourcemaps.write('.', { includeContent: false }))
//         .pipe(gulp.dest('./wwwroot/dist/js'));
// });

gulp.task('ts', function () {
   return gulp.src('./wwwroot/ts/**/*.ts')
       .pipe(webpack(require('./webpack.config.js')))
       .pipe(gulp.dest('./wwwroot/dist/js/'));
});



// watch changes in scss files and run sass task
gulp.task('sass:watch', function () { gulp.watch('./wwwroot/src/sass/**/*.scss', gulp.series('sass')); });

gulp.task('ts:watch', function () { gulp.watch('./wwwroot/src/ts/**/*.ts', gulp.series('ts')); });

gulp.task('img:watch', function () { gulp.watch('./wwwroot/src/img/**', gulp.series('img')); });

// gulp.task('clean', function () { return deleteAsync(['./wwwroot/dist/js', './wwwroot/dist/css']) });

gulp.task('watch', gulp.parallel('sass:watch', 'ts:watch'));

// default task
gulp.task('default', gulp.series('sass', 'ts'));