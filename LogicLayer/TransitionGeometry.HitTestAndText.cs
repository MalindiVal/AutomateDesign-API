using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Classe partielle gérant le hit test et le positionnement du texte des transitions
    /// </summary>
    public partial class TransitionGeometry
    {
        /// <summary>
        /// Passe a la VM l'orientation du texte
        /// </summary>
        /// <param name="index">numéro de la transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">Rayon de l'état d'arrivée</param>
        /// <returns>L'orientation du texte</returns>
        public double GetTextAngle(int index, int total, double rayon)
        {
            double angle = 0;
            if (!transition.IsLoop)
            {
                LinePoints pts = GetLinePoints(index, total, rayon);
                double dx = pts.X2 - pts.X1;
                double dy = pts.Y2 - pts.Y1;
                angle = Math.Atan2(dy, dx) * 180.0 / Math.PI;
                if (angle > 90 || angle < -90)
                {
                    angle += 180;
                }
            }
            return angle;
        }

        /// <summary>
        /// Donne la position du texte a la VM
        /// </summary>
        /// <param name="isLoop">(bool)Est une boucle</param>
        /// <param name="index">numéro de la transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">Rayon de l'état d'arrivée</param>
        /// <returns>la position X et Y (double) du texte</returns>
        public PointD GetTextPosition(bool isLoop, int index, int total, double rayon)
        {
            BezierPoints bez;
            if (isLoop)
            {
                bez = GetBezierLoopPoints(index, total, rayon);
            }
            else
            {
                bez = GetBezierPoints(index, total, rayon);
            }
            double t = 0.5;
            double x = Math.Pow(1 - t, 2) * bez.StartX + 2 * (1 - t) * t * bez.ControlX + Math.Pow(t, 2) * bez.EndX;
            double y = Math.Pow(1 - t, 2) * bez.StartY + 2 * (1 - t) * t * bez.ControlY + Math.Pow(t, 2) * bez.EndY;
            double offsetX = 0;
            double offsetY = -15;
            if (isLoop)
            {
                offsetX = -80;
                offsetY = -20;
            }
            return new PointD(x + offsetX, y + offsetY);
        }

        /// <summary>
        /// Sert a dessiner positionner le texte de la transition par rapport au segment entre les deux centre des états
        /// </summary>
        /// <param name="index">numéro de la transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">Rayon de l'état</param>
        /// <returns>renvoie les coordonnées de la lignes centrales décalé par rapport a l'index</returns>
        public LinePoints GetLinePoints(int index, int total, double rayon)
        {
            double x1 = transition.GetXDepartDecale(index, total, rayon);
            double y1 = transition.GetYDepartDecale(index, total, rayon);
            double x2 = transition.GetXArriveeDecale(index, total, rayon);
            double y2 = transition.GetYArriveeDecale(index, total, rayon);
            return new LinePoints(x1, y1, x2, y2);
        }



        /// <summary>
        /// Vérifie si un point touche la courbe de Bézier (par échantillonnage).
        /// </summary>
        /// <param name="p">Le point à tester.</param>
        /// <param name="tolerance">La tolérance en pixels pour considérer un contact.</param>
        /// <param name="index">Indice de la transition (pour les transitions multiples).</param>
        /// <param name="total">Nombre total de transitions entre les deux états.</param>
        /// <param name="rayon">Rayon du cercle représentant l’état.</param>
        /// <returns><c>true</c> si le point est proche de la transition, sinon <c>false</c>.</returns>
        public bool Contains(Point p, double tolerance, int index, int total, double rayon)
        {
            BezierPoints bez;
            if (transition.IsLoop)
            {
                bez = GetBezierLoopPoints(index, total, rayon);
            }
            else
            {
                bez = GetBezierPoints(index, total, rayon);
            }
            int steps = 40;
            double minDist = double.MaxValue;
            for (int i = 0; i <= steps; i++)
            {
                double t = i / (double)steps;
                double x = Math.Pow(1 - t, 2) * bez.StartX + 2 * (1 - t) * t * bez.ControlX + Math.Pow(t, 2) * bez.EndX;
                double y = Math.Pow(1 - t, 2) * bez.StartY + 2 * (1 - t) * t * bez.ControlY + Math.Pow(t, 2) * bez.EndY;
                double dx = x - p.X;
                double dy = y - p.Y;
                double dist = Math.Sqrt(dx * dx + dy * dy);
                if (dist < minDist)
                    minDist = dist;
            }
            return minDist <= tolerance;
        }

        /// <summary>
        /// Calcule la boîte englobante d'une boucle.
        /// </summary>
        /// <param name="index">Index de la boucle.</param>
        /// <param name="total">Nombre total de boucles.</param>
        /// <param name="rayon">Rayon de l'état.</param>
        /// <returns>Boîte englobante de la boucle.</returns>
        public BoundingBox GetLoopBoundingBox(int index, int total, double rayon)
        {
            BezierPoints points = GetBezierLoopPoints(index, total, rayon);
            return CalculExtrema(points);
        }


        /// <summary>
        /// Calcule la boîte englobante d'une courbe de Bézier.
        /// </summary>
        /// <param name="index">Index de la transition.</param>
        /// <param name="total">Nombre total de transitions.</param>
        /// <param name="rayon">Rayon de l'état d'arrivée.</param>
        /// <returns>Boîte englobante de la courbe.</returns>
        public BoundingBox GetBezierBoundingBox(int index, int total, double rayon)
        {
            BezierPoints points = GetBezierPoints(index, total, rayon);
            return CalculExtrema(points);            
        }

        /// <summary>
        /// Calcule les extrema (min/max) d'une courbe de Bézier quadratique.
        /// </summary>
        /// <param name="points">Points de la courbe (début, contrôle, fin).</param>
        /// <returns>Boîte englobante avec coordonnées min/max.</returns>
        public BoundingBox CalculExtrema(BezierPoints points)
        {
            double x0 = points.StartX;
            double y0 = points.StartY;
            double x1 = points.ControlX;
            double y1 = points.ControlY;
            double x2 = points.EndX;
            double y2 = points.EndY;          
            List<double> xValues = new List<double> { x0, x2 };
            List<double> yValues = new List<double> { y0, y2 };
            // Vérification des extrema sur X
            double denomX = x0 - 2 * x1 + x2;
            if (Math.Abs(denomX) > 0.0001)
            {
                double tX = (x0 - x1) / denomX;
                if (tX > 0 && tX < 1)
                {
                    double xAtT = (1 - tX) * (1 - tX) * x0 + 2 * (1 - tX) * tX * x1 + tX * tX * x2;
                    xValues.Add(xAtT);
                }
            }

            // Vérification des extrema sur Y
            double denomY = y0 - 2 * y1 + y2;
            if (Math.Abs(denomY) > 0.0001)
            {
                double tY = (y0 - y1) / denomY;
                if (tY > 0 && tY < 1)
                {
                    double yAtT = (1 - tY) * (1 - tY) * y0 + 2 * (1 - tY) * tY * y1 + tY * tY * y2;
                    yValues.Add(yAtT);
                }
            }
            // Prendre le vrai min/max de tous les points
            double minX = xValues.Min();
            double maxX = xValues.Max();
            double minY = yValues.Min();
            double maxY = yValues.Max();

            return new BoundingBox(minX, minY, maxX, maxY);
        }
    }
}
