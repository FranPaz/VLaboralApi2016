using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace VLaboralApi.Models
{
    

    //public class Location
    //{
    //    /// <summary>
    //    /// Latitude.
    //    /// </summary>
    //    public double Latitude { get; set; }

    //    /// <summary>
    //    /// Longitude
    //    /// </summary>
    //    public double Longitude { get; set; }

    //    /// <summary>
    //    /// Contructor intializing a valid Location
    //    /// </summary>
    //    /// <param name="latitude"></param>
    //    /// <param name="longitude"></param>
    //    public Location(double latitude, double longitude)
    //    {
    //        this.Latitude = latitude;
    //        this.Longitude = longitude;
    //    }

    //    /// <summary>
    //    /// Location expressed as Google compatible string.
    //    /// </summary>
    //    public virtual string LocationString
    //    {
    //        get
    //        {
    //            return this.Latitude.ToString(CultureInfo.InvariantCulture) + "," +
    //                   this.Longitude.ToString(CultureInfo.InvariantCulture);
    //        }
    //    }

    //    /// <summary>
    //    /// Overrdden ToString method for default conversion to Google compatible string.
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string ToString()
    //    {
    //        return this.LocationString;
    //    }
    //}

    //public class AddressLocation 
    //{
    //    /// <summary>
    //    /// Address.
    //    /// </summary>
    //    public string Address { get; protected set; }
    //    /// <summary>
    //    /// Address expressed as Google compatible string.
    //    /// </summary>
    //    public virtual string LocationString
    //    {
    //        get { return this.Address; }
    //    }

    //    /// <summary>
    //    /// Constructor initializing a valid AddressLocation
    //    /// </summary>
    //    /// <param name="address"></param>
    //    public AddressLocation(string address)
    //    {
    //        this.Address = address;
    //    }

    //    /// <summary>
    //    /// Returns locations as Google formatted locationstring. 
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string ToString()
    //    {
    //        return this.LocationString;
    //    }
    //}


    //public class AddressComponent
    //{
    //    /// <summary>
    //    /// types[] is an array indicating the type of the address component.
    //    /// </summary>
    //    public virtual IEnumerable<LocationType> Types { get; set; }

      
    //    internal virtual IEnumerable<string> TypesStr
    //    {
    //        get
    //        {
    //            return Enum.GetNames(typeof(LocationType));
    //        }
    //        //set
    //        //{              
    //        //    this.Types = value.Select(x => x.ToEnum<LocationType>());
    //        //}
    //    }

    //    /// <summary>
    //    /// short_name is an abbreviated textual name for the address component, if available. For example, an address component for the state of Alaska may have a long_name of "Alaska" and a short_name of "AK" using the 2-letter postal abbreviation.
    //    /// </summary>
    //    public virtual string ShortName { get; set; }

    //    /// <summary>
    //    /// long_name is the full text description or name of the address component as returned by the Geocoder.
    //    /// </summary>
    //    public virtual string LongName { get; set; }
    //}


    //public enum LocationType
    //{
    //    /// <summary>
    //    /// Indicates a precise street address.
    //    /// </summary>
    //    [EnumMember(Value = "street_address")]
    //    StreetAddress,
    //    /// <summary>
    //    /// Indicates a named route (such as "US 101").
    //    /// </summary>
    //    [EnumMember(Value = "route")]
    //    Route,
    //    /// <summary>
    //    /// Indicates a major intersection, usually of two major roads.
    //    /// </summary>
    //    [EnumMember(Value = "intersection")]
    //    Intersection,
    //    /// <summary>
    //    /// Indicates a political entity. Usually, this type indicates a polygon of some civil administration.
    //    /// </summary>
    //    [EnumMember(Value = "political")]
    //    Political,
    //    /// <summary>
    //    /// Indicates the national political entity, and is typically the highest order type returned by the Geocoder.
    //    /// </summary>
    //    [EnumMember(Value = "country")]
    //    Country,
    //    /// <summary>
    //    /// Indicates a first-order civil entity below the country level. Within the United States, these administrative levels are states. Not all nations exhibit these administrative levels.
    //    /// </summary>
    //    [EnumMember(Value = "administrative_area_level_1")]
    //    AdministrativeAreaLevel1,
    //    /// <summary>
    //    /// Indicates a second-order civil entity below the country level. Within the United States, these administrative levels are counties. Not all nations exhibit these administrative levels.
    //    /// </summary>
    //    [EnumMember(Value = "administrative_area_level_2")]
    //    AdministrativeAreaLevel2,
    //    /// <summary>
    //    /// Indicates a third-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    //    /// </summary>
    //    [EnumMember(Value = "administrative_area_level_3")]
    //    AdministrativeAreaLevel3,
    //    /// <summary>
    //    /// Indicates a fourth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    //    /// </summary>
    //    [EnumMember(Value = "administrative_area_level_4")]
    //    AdministrativeAreaLevel4,
    //    /// <summary>
    //    /// Indicates a fifth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    //    /// </summary>
    //    [EnumMember(Value = "administrative_area_level_5")]
    //    AdministrativeAreaLevel5,
    //    /// <summary>
    //    /// Indicates a commonly-used alternative name for the entity.
    //    /// </summary>
    //    [EnumMember(Value = "colloquial_area")]
    //    ColloquialArea,
    //    /// <summary>
    //    /// Ward indicates a specific type of Japanese locality, to facilitate distinction between multiple locality components within a Japanese address.
    //    /// </summary>
    //    [EnumMember(Value = "ward")]
    //    Ward,
    //    /// <summary>
    //    /// Indicates an incorporated city or town political entity.
    //    /// </summary>
    //    [EnumMember(Value = "locality")]
    //    Locality,
    //    /// <summary>
    //    /// indicates an first-order civil entity below a locality
    //    /// </summary>
    //    [EnumMember(Value = "sublocality")]
    //    Sublocality,
    //    /// <summary>
    //    /// indicates an first-order civil entity below a locality
    //    /// </summary>
    //    [EnumMember(Value = "sublocality_level_1")]
    //    SublocalityLevel1,
    //    /// <summary>
    //    /// indicates an second-order civil entity below a locality
    //    /// </summary>
    //    [EnumMember(Value = "sublocality_level_2")]
    //    SublocalityLevel2,
    //    /// <summary>
    //    /// indicates an third-order civil entity below a locality
    //    /// </summary>
    //    [EnumMember(Value = "sublocality_level_3")]
    //    SublocalityLevel3,
    //    /// <summary>
    //    /// indicates an first-order civil entity below a locality
    //    /// </summary>
    //    [EnumMember(Value = "sublocality_level_4")]
    //    SublocalityLevel4,
    //    /// <summary>
    //    /// indicates an first-order civil entity below a locality
    //    /// </summary>
    //    [EnumMember(Value = "sublocality_level_5")]
    //    SublocalityLevel5,
    //    /// <summary>
    //    /// Indicates a named neighborhood
    //    /// </summary>
    //    [EnumMember(Value = "neighborhood")]
    //    Neighborhood,
    //    /// <summary>
    //    /// Indicates a named location, usually a building or collection of buildings with a common name
    //    /// </summary>
    //    [EnumMember(Value = "premise")]
    //    Premise,
    //    /// <summary>
    //    /// Indicates a first-order entity below a named location, usually a singular building within a collection of buildings with a common name
    //    /// </summary>
    //    [EnumMember(Value = "subpremise")]
    //    Subpremise,
    //    /// <summary>
    //    /// Indicates a postal code as used to address postal mail within the country.
    //    /// </summary>
    //    [EnumMember(Value = "postal_code")]
    //    PostalCode,
    //    /// <summary>
    //    /// Indicates a prominent natural feature.
    //    /// </summary>
    //    [EnumMember(Value = "natural_feature")]
    //    NaturalFeature,
    //    /// <summary>
    //    /// Indicates an airport.
    //    /// </summary>
    //    [EnumMember(Value = "airport")]
    //    Airport,
    //    /// <summary>
    //    /// Indicates a named park.
    //    /// </summary>
    //    [EnumMember(Value = "park")]
    //    Park,
    //    /// <summary>
    //    /// Indicates a named point of interest. Typically, these "POI"s are prominent local entities that don't easily fit in another category such as "Empire State Building" or "Statue of Liberty."
    //    /// </summary>
    //    [EnumMember(Value = "point_of_interest")]
    //    PointOfInterest,
    //    /// <summary>
    //    /// Indicates the floor of a building address.
    //    /// </summary>
    //    [EnumMember(Value = "floor")]
    //    Floor,
    //    /// <summary>
    //    /// Establishment typically indicates a place that has not yet been categorized.
    //    /// </summary>
    //    [EnumMember(Value = "establishment")]
    //    Establishment,
    //    /// <summary>
    //    /// Parking indicates a parking lot or parking structure.
    //    /// </summary>
    //    [EnumMember(Value = "parking")]
    //    Parking,
    //    /// <summary>
    //    /// post_box indicates a specific postal box.
    //    /// </summary>
    //    [EnumMember(Value = "post_box")]
    //    PostBox,
    //    /// <summary>
    //    /// postal_town indicates a grouping of geographic areas, such as locality and sublocality, used for mailing addresses in some countries.
    //    /// </summary>
    //    [EnumMember(Value = "postal_town")]
    //    PostalTown,
    //    /// <summary>
    //    /// room indicates the room of a building address.
    //    /// </summary>
    //    [EnumMember(Value = "room")]
    //    Room,
    //    /// <summary>
    //    /// street_number indicates the precise street number.
    //    /// </summary>
    //    [EnumMember(Value = "street_number")]
    //    StreetNumber,
    //    /// <summary>
    //    /// Indicate the location of a bus stop.
    //    /// </summary>
    //    [EnumMember(Value = "bus_station")]
    //    BusStation,
    //    /// <summary>
    //    /// Indicate the location of a train stop.
    //    /// </summary>
    //    [EnumMember(Value = "train_station")]
    //    TrainStation,
    //    /// <summary>
    //    /// Indicate the location of a public transit stop.
    //    /// </summary>
    //    [EnumMember(Value = "transit_station")]
    //    TransitStation,
    //}
}