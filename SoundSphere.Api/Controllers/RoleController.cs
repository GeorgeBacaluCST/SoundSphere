﻿using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) => _roleService = roleService;

        [HttpGet] public IActionResult GetAll() => Ok(_roleService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_roleService.GetById(id));

        [HttpPost] public IActionResult Add(Role role)
        {
            Role addedRole = _roleService.Add(role);
            return CreatedAtAction(nameof(GetById), new { addedRole.Id }, addedRole);
        }
    }
}