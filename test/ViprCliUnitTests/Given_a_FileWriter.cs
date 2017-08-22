﻿using System;
using System.IO;
using System.Linq;
using Microsoft.Its.Recipes;
using Vipr;
using Vipr.Core;
using Xunit;

namespace ViprCliUnitTests
{
    public class Given_a_FileWriter
    {
        [Fact]
        public void When_no_files_are_specified_it_returns()
        {
            FileWriter.WriteAsync(Enumerable.Empty<TextFile>());
        }

        [Fact]
        public void When_outputDirectoryPath_is_not_specified_then_it_writes_the_files_to_the_working_directory()
        {
            var files = Any.IEnumerable<TextFile>().ToList();

            try
            {
                FileWriter.WriteAsync(files);
                FileSystemHelpers.ValidateTextFiles(files);
            }
            finally
            {
                FileSystemHelpers.DeleteFiles(files);
            }
        }

        [Fact]
        public void When_outputDirectoryPath_is_specified_and_exitst_then_it_writes_the_files_to_the_outputDirectoryPath()
        {
            var files = Any.IEnumerable<TextFile>().ToList();
            var outputDirectoryPath = Any.Word();
            Directory.CreateDirectory(outputDirectoryPath);

            try
            {
                FileWriter.WriteAsync(files, outputDirectoryPath);
                FileSystemHelpers.ValidateTextFiles(files, outputDirectoryPath);
            }
            finally
            {
                FileSystemHelpers.DeleteFiles(files, outputDirectoryPath);
                Directory.Delete(outputDirectoryPath);
            }
        }

        [Fact]
        public void When_outputDirectoryPath_is_specified_and_does_not_exist_then_it_creates_the_directory_and_writes_the_files()
        {
            var files = Any.IEnumerable<TextFile>().ToList();
            var outputDirectoryPath = Any.Word();

            try
            {
                FileWriter.WriteAsync(files, outputDirectoryPath);
                FileSystemHelpers.ValidateTextFiles(files, outputDirectoryPath);
            }
            finally
            {
                FileSystemHelpers.DeleteFiles(files, outputDirectoryPath);
                Directory.Delete(outputDirectoryPath);
            }
        }

        [Fact]
        public void When_the_executing_assembly_is_executed_from_a_different_directory_files_are_written_to_that_directory()
        {
            var files = Any.IEnumerable<TextFile>().ToList();

            var currentDirectory = Environment.CurrentDirectory;

            var workingDirectory = Path.Combine(Environment.CurrentDirectory, Any.Word());

            var outputDirectory = Any.Word();

            try
            {

                Directory.CreateDirectory(workingDirectory);

                Environment.CurrentDirectory = Path.Combine(workingDirectory);

                FileWriter.WriteAsync(files, outputDirectory);

                FileSystemHelpers.ValidateTextFiles(files, outputDirectory);
            }
            finally
            {

                FileSystemHelpers.DeleteFiles(files, outputDirectory);

                Directory.Delete(outputDirectory);

                Environment.CurrentDirectory = currentDirectory;

                Directory.Delete(workingDirectory);
            }
        }
    }
}
