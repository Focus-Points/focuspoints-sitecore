var gulp = require('gulp');
var $ = require('gulp-load-plugins')();
$.runSequence = require('run-sequence');

var msbuild = require("gulp-msbuild");

var config = require("./gulp-config.js")();
var solution = "./" + config.solutionName + ".sln";

gulp.task("Build-Solution", function () {
	var targets = ["Build"];
	if (config.runCleanBuilds) {
		targets = ["Clean", "Build"];
	}

	return gulp.src(solution)
		.pipe($.msbuild({
			targets: targets,
			configuration: config.buildConfiguration,
			logCommand: false,
			verbosity: "minimal",
			stdout: true,
			errorOnFail: true,
			maxcpucount: 0,
			toolsVersion: 14.0
		}));
});

gulp.task("Build-and-Publish-Solution", function () {
	console.log("Publishing to " + config.siteRoot + " folder");

	var targets = ["Build"];

	return gulp.src(solution)
		.pipe($.msbuild({
			targets: targets,
			configuration: config.buildConfiguration,
			logCommand: false,
			verbosity: "minimal",
			stdout: true,
			errorOnFail: true,
			maxcpucount: 0,
			toolsVersion: 14.0,
			properties: {
				DeployOnBuild: "true",
				DeployDefaultTarget: "WebPublish",
				WebPublishMethod: "FileSystem",
				DeleteExistingFiles: "false",
				publishUrl: config.siteRoot,
				_FindDependencies: "false",
				WebPublishPipelineCustomizeTargetFile: config.webPublishingProfileTargets
			}
		}));
});
