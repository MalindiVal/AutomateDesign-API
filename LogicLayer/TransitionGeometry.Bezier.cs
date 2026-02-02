using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    /// <summary>
    /// Partie de la classe TransitionGeometry gérant les courbes de bézier
    /// </summary>
    public partial class TransitionGeometry
    {
        /// <summary>
        /// Donne le tracé de la courbe de bézier à la VM
        /// </summary>
        /// <param name="index">numéro de la transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">Rayon de l'état d'arrivée</param>
        /// <returns>la chaine de caractère contenant le tracé de la courbe (Point de départ, point de controle et point d'arrivé)</returns>
        public string GetBezierPathData(int index, int total, double rayon)
        {
            BezierPoints pts = GetBezierPoints(index, total, rayon);
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "M {0:F2},{1:F2} Q {2:F2},{3:F2} {4:F2},{5:F2}", pts.StartX, pts.StartY, pts.ControlX, pts.ControlY, pts.EndX, pts.EndY);
        }

        /// <summary>
        ///  Donne le tracé de la boucle utilisant une courbe de bézier à la VM
        /// </summary>
        /// <param name="index">numéro de la transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">Rayon de l'état d'arrivée</param>
        /// <returns>la chaine de caractère contenant le tracé de la boucle (Point de départ, point de controle et point d'arrivé)</returns>
        public string GetLoopPathData(int index, int total, double rayon)
        {
            BezierPoints bez = GetBezierLoopPoints(index, total, rayon);
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "M {0},{1} Q {2},{3} {4},{5}", bez.StartX, bez.StartY, bez.ControlX, bez.ControlY, bez.EndX, bez.EndY);
        }

        /// <summary>
        /// donnes les points pour la fleche 
        /// </summary>
        /// <param name="isLoop">(bool)Est une boucle</param>
        /// <param name="index">numéro de la transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">Rayon de l'état</param>
        /// <returns>renvoie les coordonnées de la lignes centrales décalé par rapport a l'index</returns>
        public string GetArrowPoints(bool isLoop, int index, int total, double rayon)
        {
            BezierPoints bez;
            double t;
            if (isLoop)
            {
                bez = GetBezierLoopPoints(index, total, rayon);
                t = 0.7;
            }
            else
            {
                bez = GetBezierPoints(index, total, rayon);
                t = 0.5;
            }
            double x = Math.Pow(1 - t, 2) * bez.StartX + 2 * (1 - t) * t * bez.ControlX + Math.Pow(t, 2) * bez.EndX;
            double y = Math.Pow(1 - t, 2) * bez.StartY + 2 * (1 - t) * t * bez.ControlY + Math.Pow(t, 2) * bez.EndY;
            double dxT = 2 * (1 - t) * (bez.ControlX - bez.StartX) + 2 * t * (bez.EndX - bez.ControlX);
            double dyT = 2 * (1 - t) * (bez.ControlY - bez.StartY) + 2 * t * (bez.EndY - bez.ControlY);
            double angle = Math.Atan2(dyT, dxT);
            Point p1 = new((int)x, (int)y);
            Point p2 = new(
                (int)(x - TailleFleche * Math.Cos(angle - Math.PI / 6)),
                (int)(y - TailleFleche * Math.Sin(angle - Math.PI / 6))
            );
            Point p3 = new(
                (int)(x - TailleFleche * Math.Cos(angle + Math.PI / 6)),
                (int)(y - TailleFleche * Math.Sin(angle + Math.PI / 6))
            );
            return $"{p1.X},{p1.Y} {p2.X},{p2.Y} {p3.X},{p3.Y}";
        }

        /// <summary>
        /// Calcul les points pour dessiner la courbe de bézier
        /// </summary>
        /// <param name="index">nombre de transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">rayon de l'état</param>
        /// <returns>Points qui servent a dessiner la courbe</returns>
        public BezierPoints GetBezierPoints(int index, int total, double rayon)
        {
            BezierPoints result;
            double x1 = transition.EtatDebut.Position.X;
            double y1 = transition.EtatDebut.Position.Y;
            double x2 = transition.EtatFinal.Position.X;
            double y2 = transition.EtatFinal.Position.Y;
            double r = rayon / 2.0;           
            double controlX, controlY;
            if (transition.HasManualControl)
            {
                controlX = transition.ManualControlX.Value;
                controlY = transition.ManualControlY.Value;
            }
            else
            {
                double dx = x2 - x1;
                double dy = y2 - y1;
                double mx = (x1 + x2) / 2;
                double my = (y1 + y2) / 2;
                double nx = -(dy);
                double ny = (dx);
                double magnitude = Math.Sqrt(nx * nx + ny * ny);
                if (magnitude == 0) magnitude = 1;
                double offset = 30 + index * 30;
                controlX = mx + nx / magnitude * offset;
                controlY = my + ny / magnitude * offset;
            }
            PointD pStart = ProjectOnCircle(x1, y1, r, controlX, controlY);
            PointD pEnd = ProjectOnCircle(x2, y2, r, controlX, controlY);
            result = new BezierPoints(pStart.X, pStart.Y, controlX, controlY, pEnd.X, pEnd.Y);
            return result;

        }

        /// <summary>
        /// Met à jour le point de contrôle selon la position de la souris.
        /// </summary>
        /// <param name="mouseX">Position X de la souris.</param>
        /// <param name="mouseY">Position Y de la souris.</param>
        /// <param name="r">Rayon des états liés.</param>
        public void UpdateControlForMouse(double mouseX, double mouseY, double r)
        {           
            double x1 = transition.EtatDebut.Position.X;
            double y1 = transition.EtatDebut.Position.Y;
            double x2 = transition.EtatFinal.Position.X;
            double y2 = transition.EtatFinal.Position.Y;
            PointD pStart = ProjectOnCircle(x1, y1, r, x2, y2);
            PointD pEnd = ProjectOnCircle(x2, y2, r, x1, y1);
            double startX = pStart.X;
            double startY = pStart.Y;
            double endX = pEnd.X;
            double endY = pEnd.Y;
            double t = 0.5;           
            double controlX = (mouseX - Math.Pow(1 - t, 2) * startX - Math.Pow(t, 2) * endX) / (2 * (1 - t) * t);
            double controlY = (mouseY - Math.Pow(1 - t, 2) * startY - Math.Pow(t, 2) * endY) / (2 * (1 - t) * t);
            transition.ManualControlX = controlX;
            transition.ManualControlY = controlY;
        }

        /// <summary>
        /// Calcule un point de la courbe de Bézier intermédiaire.
        /// </summary>
        /// <param name="isLoop">Indique s’il s’agit d’une boucle.</param>
        /// <param name="index">Index de la transition.</param>
        /// <param name="total">Nombre total de transitions.</param>
        /// <param name="rayon">Rayon des états.</param>
        /// <returns>Le point évalué sur la courbe.</returns>
        public PointD GetCurvePoint(bool isLoop, int index, int total, double rayon)
        {
            BezierPoints bez;
            if (isLoop)
                bez = GetBezierLoopPoints(index, total, rayon);
            else
                bez = GetBezierPoints(index, total, rayon);

            double t = 0.5;
            double x = Math.Pow(1 - t, 2) * bez.StartX + 2 * (1 - t) * t * bez.ControlX + Math.Pow(t, 2) * bez.EndX;
            double y = Math.Pow(1 - t, 2) * bez.StartY + 2 * (1 - t) * t * bez.ControlY + Math.Pow(t, 2) * bez.EndY;
            return new PointD(x, y);
        }

        /// <summary>
        /// Calcul les points pour dessiner la courbe de bézier (pour une boucle)
        /// </summary>
        /// <param name="index">nombre de transition</param>
        /// <param name="total">nombre total de transition</param>
        /// <param name="rayon">rayon de l'état</param>
        /// <returns>Points qui servent a dessiner la courbe</returns>
        public BezierPoints GetBezierLoopPoints(int index, int total, double rayon)
        {

            BezierPoints result;    
            double CX = transition.EtatDebut.Position.X;
            double CY = transition.EtatDebut.Position.Y;
            double loopSize = 40 + index * 20;
            double r = rayon / 2.0;
            double startX = CX + r;
            double startY = CY;
            double endX = CX;
            double endY = CY - r;
            if (transition.HasManualControl)
            {
                result =  new BezierPoints(startX, startY, transition.ManualControlX.Value, transition.ManualControlY.Value, endX, endY);
            }
            else
            {
                double controlX = CX + r + loopSize;
                double controlY = CY - r - loopSize;
                result = new BezierPoints(startX, startY, controlX, controlY, endX, endY);
            }
            return result;
           
        }

        /// <summary>
        /// Projette une position cible sur le cercle d’un état.
        /// </summary>
        /// <param name="cx">Centre X.</param>
        /// <param name="cy">Centre Y.</param>
        /// <param name="r">Rayon du cercle.</param>
        /// <param name="targetX">Cible X.</param>
        /// <param name="targetY">Cible Y.</param>
        /// <returns>Le point projeté sur le cercle.</returns>
        public PointD ProjectOnCircle(double cx, double cy, double r,double targetX, double targetY)
        {
            double dx = targetX - cx;
            double dy = targetY - cy;
            double d = Math.Sqrt(dx * dx + dy * dy);
            if (d == 0) 
            { 
                d = 1; 
            }
            double px = cx + dx / d * r;
            double py = cy + dy / d * r;
            return new PointD(px, py);
        }
    }
}
