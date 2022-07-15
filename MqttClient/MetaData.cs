using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttClient
{
    public class MetaData
    {
        public string channel_id { get; set; }                   // 채널 ID
        public DateTime timestamp { get; set; } // 발생 시간
        public Int32 image_width { get; set; }
        public Int32 image_height { get; set; }
        public List<EventInfo> event_list { get; set; }       // 이벤트 검출 리스트
        public List<Segmetation> object_list { get; set; }    // 객체 검출 
    }

    /* 
        raised event info
    */
    public class EventInfo
    {
        public Int32 id { get; set; }                          // 객체 ID
        public State state { get; set; }                       // 객체 상태
        public EventType event_type { get; set; }              // 검출 정보
        public Segmetation segmentation { get; set; }          // 검출 정보

        /*
        관심 영역 발생 내역
        */
        public class TraceResult
        {
            /*
                관심 영역 정보
            */
            public class RoiResult
            {
                public string roi_id { get; set; }
                public string roi_name { get; set; }
                /*
                    관심 영역 라인 정보 (IN, OUT)
                */
                public class LineResult
                {
                    public Int32 line_id { get; set; }
                    public string line_name { get; set; }
                }
                public LineResult alram_line { get; set; }
            }
            public RoiResult alram_roi { get; set; }

            /*
                영역 연계 정보
            */
            public class LinkResult
            {
                public string link_id { get; set; }
                public string link_name { get; set; }
            }
            public LinkResult alram_link { get; set; }
            public Int32 alram_timestamp { get; set; }
        }
        public List<TraceResult> alram_trace { get; set; }  // ROI 내역

        public class JpegImageInfo
        {
            public string base64_image { get; set; }           // 검출 객체 이미지 (jpeg image --> base64 encoding)
            public BoundingBox object_box { get; set; }
            public Int32 image_width { get; set; }
            public Int32 image_height { get; set; }
        }
        public JpegImageInfo jpeg_image { get; set; }
        public double event_score { get; set; }
        public string event_message { get; set; }
        // |event    |  event score             | event_message  |
        // |matching |  matchingScore (0.0~1.0) | mathcing uid |
        // |masked   |  msakedScore (0.0~1.0)   | none |
    }
    /*
        프레임 내 검출 결과 정보
    */
    public class Segmetation
    {
        public ObjectType label { get; set; }         // 객체 종류 (enum ObjectType 참고)
        public BoundingBox box { get; set; }          // 객체 검출 박스 좌표
        public double confidence { get; set; }        // 검출 정확도 (0 ~1)
    }

    /*
        검출 객체 정보
        좌표 기준 : 영상 비율 기준 정규화 (0 ~ 1)
    */
    public class BoundingBox
    {
        public double x { get; set; }      //객체 박스 x좌표
        public double y { get; set; }      //객체 박스 y좌표
        public double width { get; set; }  //객체 박스 너비
        public double height { get; set; } //객체 박스 높이
    }

    /* message roi */
    public class AreaList
    {
        public string channelId { get; set; }
        public DateTime timestamp { get; set; } // 발생 시간

        /*
            Area Info
        */
        public class AreaInfo
        {
            public string roiId { get; set; }
            public Int32 event_type { get; set; }
            public string event_desc { get; set; }
            public List<Int32> class_ids { get; set; }
            public List<Dot> dots { get; set; }
        }
        public List<AreaInfo> area_list { get; set; }
    }

    public class Dot
    {
        public double x { get; set; }
        public double y { get; set; }
    }
    /*
        객체 종류
    */
    public enum ObjectType
    {
        PERSON = 0,                   // 사람
        BIKE = 1,                     // 자전거
        CAR = 2,                      // 승용차
        MOTORCYCLE = 3,               // 오토바이
        BUS = 4,                      // 버스
        TRUCK = 5,                    // 트럭
        EXCAVATOR = 6,                // 굴착기
        TANKTRUCK = 7,                // 탱크트럭
        FORKLIFT = 8,                 // 지게차
        LEMICON = 9,                  // 레미콘
        CULTIVATOR = 10,              // 경운기
        TRACTOR = 11,                 // 트랙터
        FLAME = 50,                   // 불꽃
        SMOKE = 51,                   // 연기
        FACE = 200,                   // 얼굴_남자
        HELMET = 202,                 // 얼굴_헬멧
        HEAD = 203,                   // 얼굴_헬멧
    }

    public enum EventType
    {
        Default = 0,                  // None
        Loitering = 1,                // 배회 (1)
        Intrusion = 2,                // 침입 (1)
        Falldown = 3,                 // 쓰러짐 (1)
        Violence = 5,                 // 싸움,폭력 (1)
        Congestion = 10,              // 객체 혼잡도 (1)
        LineCrossing = 13,            // 양방향 라인 통과 (1)
        IllegalParking = 14,          // 불법 주정차_10분 (1)
        DirectionCount = 16,          // 방향성 이동 카운트 (좌, 우, 유턴 등)
        CongestionLevel = 17,         // 사람 혼잡도 레벨 (1)
        VehicleDensity = 19,          // 차량 밀도 (1)
        StopVehicleCount = 20,        // 정차 중인 차량 수 카운트 (1)
        Longstay = 22,                // 체류
        LineEnter = 23,               // 단방향 라인 통과(1)
        FireSmoke = 25,               // 화재 연기 (1)
        FireFlame = 26,               // 화재 불꽃 (1)
        MatchingFace = 27,            // 등록 얼굴 매칭 (1)
        UnMaskedFace = 28,            // 얼굴 마스크 미착용 (1)
        NoHelmetHead = 29,            // 헬멧 미착용
        HelmetHead = 30,              // 헬멧 착용
    }

    /*
        이벤트 발생 상태값
    */
    public enum State
    {
        START = 0,      //시작
        CONTINUE = 1,   //진행
        END = 2,        //종료
    }
}
