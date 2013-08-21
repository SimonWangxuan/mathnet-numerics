// <copyright file="MatrixLoader.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2013 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Random;
using NUnit.Framework;

namespace MathNet.Numerics.UnitTests.LinearAlgebraTests.Double
{
    /// <summary>
    /// Base class for matrix tests.
    /// </summary>
    public abstract class MatrixLoader
    {
        /// <summary>
        /// Gets or sets test matrices values to use.
        /// </summary>
        protected Dictionary<string, double[,]> TestData2D { get; set; }

        /// <summary>
        /// Gets or sets test matrices instances to use.
        /// </summary>
        protected Dictionary<string, Matrix> TestMatrices { get; set; }

        /// <summary>
        /// Creates a matrix for the given number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <returns>A matrix with the given dimensions.</returns>
        protected abstract Matrix CreateMatrix(int rows, int columns);

        /// <summary>
        /// Creates a matrix from a 2D array.
        /// </summary>
        /// <param name="data">The 2D array to create this matrix from.</param>
        /// <returns>A matrix with the given values.</returns>
        protected abstract Matrix CreateMatrix(double[,] data);

        /// <summary>
        /// Creates a vector of the given size.
        /// </summary>
        /// <param name="size">The size of the vector to create.
        /// </param>
        /// <returns>The new vector. </returns>
        protected abstract Vector CreateVector(int size);

        /// <summary>
        /// Creates a vector from an array.
        /// </summary>
        /// <param name="data">The array to create this vector from.</param>
        /// <returns>The new vector. </returns>
        protected abstract Vector CreateVector(double[] data);

        /// <summary>
        /// Setup test matrices.
        /// </summary>
        [SetUp]
        public virtual void SetupMatrices()
        {
            TestData2D = new Dictionary<string, double[,]>
                {
                    {"Singular3x3", new[,] {{1.0, 1.0, 2.0}, {1.0, 1.0, 2.0}, {1.0, 1.0, 2.0}}},
                    {"Square3x3", new[,] {{-1.1, -2.2, -3.3}, {0.0, 1.1, 2.2}, {-4.4, 5.5, 6.6}}},
                    {"Square4x4", new[,] {{-1.1, -2.2, -3.3, -4.4}, {0.0, 1.1, 2.2, 3.3}, {1.0, 2.1, 6.2, 4.3}, {-4.4, 5.5, 6.6, -7.7}}},
                    {"Singular4x4", new[,] {{-1.1, -2.2, -3.3, -4.4}, {-1.1, -2.2, -3.3, -4.4}, {-1.1, -2.2, -3.3, -4.4}, {-1.1, -2.2, -3.3, -4.4}}},
                    {"Tall3x2", new[,] {{-1.1, -2.2}, {0.0, 1.1}, {-4.4, 5.5}}},
                    {"Wide2x3", new[,] {{-1.1, -2.2, -3.3}, {0.0, 1.1, 2.2}}},
                    {"Symmetric3x3", new[,] {{1.0, 2.0, 3.0}, {2.0, 2.0, 0.0}, {3.0, 0.0, 3.0}}}
                };

            TestMatrices = new Dictionary<string, Matrix>();
            foreach (var name in TestData2D.Keys)
            {
                TestMatrices.Add(name, CreateMatrix(TestData2D[name]));
            }
        }

        /// <summary>
        /// Creates a <c>DenseMatrix</c> with random values.
        /// </summary>
        /// <param name="row">The number of rows.</param>
        /// <param name="col">The number of columns.</param>
        /// <returns>A <c>DenseMatrix</c> with the given dimensions and random values.</returns>
        public static Matrix GenerateRandomDenseMatrix(int row, int col)
        {
            // Fill a matrix with standard random numbers.
            var normal = new Normal(new MersenneTwister(1));
            var matrixA = new DenseMatrix(row, col);
            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    matrixA[i, j] = normal.Sample();
                }
            }

            return matrixA;
        }

        /// <summary>
        /// Creates a positive definite <c>DenseMatrix</c> with random values.
        /// </summary>
        /// <param name="order">The order of the matrix.</param>
        /// <returns>A positive definite <c>DenseMatrix</c> with the given order and random values.</returns>
        public static Matrix<double> GenerateRandomPositiveDefiniteDenseMatrix(int order)
        {
            // Fill a matrix with standard random numbers.
            var normal = new Normal(new MersenneTwister(1));
            var matrixA = new DenseMatrix(order);
            for (var i = 0; i < order; i++)
            {
                for (var j = 0; j < order; j++)
                {
                    matrixA[i, j] = normal.Sample();
                }
            }

            // Generate a matrix which is positive definite.
            return matrixA.Transpose()*matrixA;
        }

        /// <summary>
        /// Creates a <c>DenseVector</c> with random values.
        /// </summary>
        /// <param name="order">The size of the vector.</param>
        /// <returns>A <c>DenseVector</c> with the given dimension and random values.</returns>
        public static Vector GenerateRandomDenseVector(int order)
        {
            // Fill a matrix with standard random numbers.
            var normal = new Normal(new MersenneTwister(1));
            var v = new DenseVector(order);
            for (var i = 0; i < order; i++)
            {
                v[i] = normal.Sample();
            }

            return v;
        }

        /// <summary>
        /// Creates a <c>UserDefinedMatrix</c> with random values.
        /// </summary>
        /// <param name="row">The number of rows.</param>
        /// <param name="col">The number of columns.</param>
        /// <returns>A <c>UserDefinedMatrix</c> with the given dimensions and random values.</returns>
        public static Matrix GenerateRandomUserDefinedMatrix(int row, int col)
        {
            // Fill a matrix with standard random numbers.
            var normal = new Normal(new MersenneTwister(1));
            var matrixA = new UserDefinedMatrix(row, col);
            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    matrixA[i, j] = normal.Sample();
                }
            }

            return matrixA;
        }

        /// <summary>
        /// Creates a positive definite <c>UserDefinedMatrix</c> with random values.
        /// </summary>
        /// <param name="order">The order of the matrix.</param>
        /// <returns>A positive definite <c>UserDefinedMatrix</c> with the given order and random values.</returns>
        public static Matrix<double> GenerateRandomPositiveDefiniteUserDefinedMatrix(int order)
        {
            // Fill a matrix with standard random numbers.
            var normal = new Normal(new MersenneTwister(1));
            var matrixA = new UserDefinedMatrix(order);
            for (var i = 0; i < order; i++)
            {
                for (var j = 0; j < order; j++)
                {
                    matrixA[i, j] = normal.Sample();
                }
            }

            // Generate a matrix which is positive definite.
            return matrixA.Transpose()*matrixA;
        }

        /// <summary>
        /// Creates a <c>UserDefinedVector</c> with random values.
        /// </summary>
        /// <param name="order">The size of the vector.</param>
        /// <returns>A <c>UserDefinedVector</c> with the given dimension and random values.</returns>
        public static Vector GenerateRandomUserDefinedVector(int order)
        {
            // Fill a matrix with standard random numbers.
            var normal = new Normal(new MersenneTwister(1));
            var v = new UserDefinedVector(order);
            for (var i = 0; i < order; i++)
            {
                v[i] = normal.Sample();
            }

            // Generate a matrix which is positive definite.
            return v;
        }
    }
}
