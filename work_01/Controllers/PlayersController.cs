﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using work_01.Models;
using work_01.Models.ViewModels;
using PagedList;

namespace work_01.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ClubDbContext db = new ClubDbContext();

        // GET: Players
        public ActionResult Index(int page=1)
        {
            var players = db.Players.Include("Sports");
            return View(players.OrderBy(x => x.PlayerId).ToPagedList(page,5));
        }
        public ActionResult Create()
        {
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(PlayerViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                if (pvm.Picture != null)
                {
                    //for Image save
                    string filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    pvm.Picture.SaveAs(Server.MapPath(filePath));

                    //for player
                    Player player = new Player
                    {
                        PlayerName = pvm.PlayerName,
                        JoinDate = pvm.JoinDate,
                        PlayerGrade = pvm.PlayerGrade,
                        SportsId = pvm.SportsId,
                        PicturePath = filePath
                    };
                    db.Players.Add(player);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View(pvm);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            PlayerViewModel pvm = new PlayerViewModel
            {
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                JoinDate = player.JoinDate,
                PlayerGrade = player.PlayerGrade,
                SportsId = player.SportsId,
                PicturePath = player.PicturePath
            };
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View(pvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlayerViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                string filePath = pvm.PicturePath;
                if (pvm.Picture != null)
                {
                    filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(pvm.Picture.FileName));
                    pvm.Picture.SaveAs(Server.MapPath(filePath));

                    Player player = new Player
                    {
                        PlayerId=pvm.PlayerId,
                        PlayerName = pvm.PlayerName,
                        JoinDate = pvm.JoinDate,
                        PlayerGrade = pvm.PlayerGrade,
                        SportsId = pvm.SportsId,
                        PicturePath = filePath
                    };
                    db.Entry(player).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    Player player = new Player
                    {
                        PlayerId = pvm.PlayerId,
                        PlayerName = pvm.PlayerName,
                        JoinDate = pvm.JoinDate,
                        PlayerGrade = pvm.PlayerGrade,
                        SportsId = pvm.SportsId,
                        PicturePath = filePath
                    };
                    db.Entry(player).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName",pvm.SportsId);
            return View(pvm);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            Player player=db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}