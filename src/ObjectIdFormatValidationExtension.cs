﻿using MongoDB.Bson;

namespace Identity.Business
{
    public static class ObjectIdFormatValidationExtension
    {
        public static bool IsValidObjectId(this string value)
        {
            try
            {
                var _ = new ObjectId(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}