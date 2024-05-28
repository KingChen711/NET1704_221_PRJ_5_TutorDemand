﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class TutorBusiness : ITutorBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public TutorBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> FindOneAsync(Expression<Func<Tutor, bool>> expression)
        {
            try
            {
                var tutors = await _unitOfWork.TutorRepository.GetWithConditionAsync(expression);

                if (tutors != null)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, tutors);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> CreateAsync(TutorAddDto dto)
        {
            try
            {
                var entity = dto.Adapt<Tutor>();
                _unitOfWork.TutorRepository.Create(entity);
                var result = await _unitOfWork.TutorRepository.SaveAsync() > 0;
                if (result)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteAsync(Guid tutorId)
        {
            var tutorEntity = await _unitOfWork.TutorRepository.GetByIdAsync(tutorId);
            if (tutorEntity is null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                await _unitOfWork.TutorRepository.RemoveAsync(tutorEntity);
                await _unitOfWork.TutorRepository.SaveAsync();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<IBusinessResult> GetAllAsync()
        {
            try
            {
                var tutorEntities = await _unitOfWork.TutorRepository.GetAllAsync();

                if (tutorEntities != null)
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, tutorEntities);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateAsync(TutorUpdateDto dto)
        {
            try
            {
                var entity = dto.Adapt<Tutor>();
                await _unitOfWork.TutorRepository.UpdateAsync(entity);
                var result = await _unitOfWork.TutorRepository.SaveAsync() > 0;

                if (result)
                {
                    return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }
    }
};

